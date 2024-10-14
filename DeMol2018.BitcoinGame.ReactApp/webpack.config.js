const path = require('path');
const webpack = require('webpack');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const TerserPlugin = require('terser-webpack-plugin');
const HtmlWebpackPlugin = require("html-webpack-plugin");

module.exports = (env) => {
    const isDevBuild = true;// !(env && env.prod);
    const clientBundleOutputDir = './public/dist';

    const clientBundleConfig = {
        mode: isDevBuild ? "development" : "production",
        entry: {
            'vendor': [
                'bootstrap/dist/css/bootstrap.css',
                'event-source-polyfill',
                'react',
                'react-redux',
                'redux',
                'redux-thunk',
                'jquery'
            ],
            'main-client': './ClientApp/boot-client.tsx',
        },
        module: {
            rules: [
                { test: /\.css$/, use: [ MiniCssExtractPlugin.loader, isDevBuild ? 'css-loader' : 'css-loader?minimize' ]},
                { test: /\.tsx?$/, include: /ClientApp/, use: [{ loader: 'ts-loader', options: { transpileOnly: true }}] },
                { test: /\.(png|jpg|jpeg|gif|svg)$/, use: 'url-loader?limit=25000' },
                { test: /\.ttf$/, use: 'url-loader?limit=25000&name=[hash].[ext]' }
            ]
        },
        output: {
            filename: '[name].js',
            path: path.join(__dirname, clientBundleOutputDir) },

        plugins: [
            new MiniCssExtractPlugin(),
            new HtmlWebpackPlugin({
                template: "public/index.html",
            }),
            new webpack.ProvidePlugin({ $: 'jquery', jQuery: 'jquery' }), // Maps these identifiers to the jQuery package (because Bootstrap expects it to be a global variable)
            new webpack.NormalModuleReplacementPlugin(/\/iconv-loader$/, require.resolve('node-noop')), // Workaround for https://github.com/andris9/encoding/issues/16
            new webpack.DefinePlugin({
                'process.env.NODE_ENV': isDevBuild ? '"development"' : '"production"'
            }),
        ].concat(isDevBuild ? [
            // Plugins that apply in development builds only
            new webpack.SourceMapDevToolPlugin({
                filename: '[file].map', // Remove this line if you prefer inline source maps
                moduleFilenameTemplate: path.relative(clientBundleOutputDir, '[resourcePath]'), // Point sourcemap entries to the original file locations on disk
                exclude: ['vendor.js']
            })
        ] : [
            // Plugins that apply in production builds only
            new TerserPlugin({
                parallel: {
                    cache: true,
                    workers: 2
                }
            })
        ]),
        stats: { modules: false },
        resolve: { extensions: ['.js', '.jsx', '.ts', '.tsx'] },
    };

    return [clientBundleConfig];
};
