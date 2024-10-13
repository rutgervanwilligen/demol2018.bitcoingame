const path = require('path');
const webpack = require('webpack');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const { merge } = require('webpack-merge');
const TerserPlugin = require('terser-webpack-plugin');
const HtmlWebpackPlugin = require("html-webpack-plugin");

module.exports = (env) => {
    const isDevBuild = true;// !(env && env.prod);

    // Configuration in common to both client-side and server-side bundles
    const sharedConfig = () => ({
        mode: isDevBuild ? "development" : "production",
        stats: { modules: false },
        resolve: { extensions: ['.js', '.jsx', '.ts', '.tsx'] },
        output: {
            filename: '[name].js',
            path: '/dist' // Webpack dev middleware, if enabled, handles requests for this URL prefix
        },
        module: {
            rules: [
                { test: /\.tsx?$/, include: /ClientApp/, use: [{ loader: 'ts-loader', options: { transpileOnly: true }}] },
                { test: /\.(png|jpg|jpeg|gif|svg)$/, use: 'url-loader?limit=25000' },
                { test: /\.ttf$/, use: 'url-loader?limit=25000&name=[hash].[ext]' }
            ]
        },
        // plugins: [new CheckerPlugin()]
    });

    // Configuration for client-side bundle suitable for running in browsers
    const clientBundleOutputDir = './public/dist';
    const clientBundleConfig = merge(sharedConfig(), {
        entry: { 'main-client': './ClientApp/boot-client.tsx' },
        module: {
            rules: [
                {
                    test: /\.css$/,
                    use: [ MiniCssExtractPlugin.loader, isDevBuild ? 'css-loader' : 'css-loader?minimize' ]
                }
            ]
        },
        output: { path: path.join(__dirname, clientBundleOutputDir) },
        plugins: [
            new MiniCssExtractPlugin(),
            // new webpack.DllReferencePlugin({
            //     context: __dirname,
            //     manifest: require('./wwwroot/dist/vendor-manifest.json')
            // })
            new HtmlWebpackPlugin({
                template: "public/index.html",
            }),
        ].concat(isDevBuild ? [
            // Plugins that apply in development builds only
            new webpack.SourceMapDevToolPlugin({
                filename: '[file].map', // Remove this line if you prefer inline source maps
                moduleFilenameTemplate: path.relative(clientBundleOutputDir, '[resourcePath]') // Point sourcemap entries to the original file locations on disk
            })
        ] : [
            // Plugins that apply in production builds only
            new TerserPlugin({
                parallel: {
                    cache: true,
                    workers: 2
                }
            })
        ])
    });

    // Configuration for server-side (prerendering) bundle suitable for running in Node
    // const serverBundleConfig = merge(sharedConfig(), {
    //     resolve: { mainFields: ['main'] },
    //     entry: { 'main-server': './ClientApp/boot-server.tsx' },
    //     plugins: [
    //         new webpack.DllReferencePlugin({
    //             context: __dirname,
    //             manifest: require('./ClientApp/dist/vendor-manifest.json'),
    //             sourceType: 'commonjs2',
    //             name: './vendor'
    //         })
    //     ],
    //     output: {
    //         libraryTarget: 'commonjs',
    //         path: path.join(__dirname, './ClientApp/dist')
    //     },
    //     target: 'node',
    //     devtool: 'inline-source-map'
    // });

    return [clientBundleConfig ];//, serverBundleConfig];
};
