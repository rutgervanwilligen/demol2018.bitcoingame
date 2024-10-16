const path = require('path');
const webpack = require('webpack');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const ReactRefreshWebpackPlugin = require('@pmmmwh/react-refresh-webpack-plugin');
const ReactRefreshTypeScript = require('react-refresh-typescript');

const common = require("./webpack.common.js");
const { merge } = require("webpack-merge");

const clientBundleOutputDir = './public/dist';

module.exports = merge(common, {
    mode: "development",
    devServer: {
        hot: true,
    },
    module: {
        rules: [
            { test: /\.css$/, use: [ MiniCssExtractPlugin.loader, 'css-loader' ]},
            { test: /\.tsx?$/, include: /ClientApp/, use: [
                    {
                        loader: require.resolve('ts-loader'),
                        options: {
                            getCustomTransformers: () => ({
                                before: [ReactRefreshTypeScript()].filter(Boolean),
                            }),
                            transpileOnly: true,
                        },
                    }
                ]},
            { test: /\.(png|jpg|jpeg|gif|svg)$/, use: 'url-loader?limit=25000' },
            { test: /\.ttf$/, use: 'url-loader?limit=25000&name=[hash].[ext]' }
        ]
    },
    plugins: [
        new webpack.SourceMapDevToolPlugin({
            filename: '[file].map',
            moduleFilenameTemplate: path.relative(clientBundleOutputDir, '[resourcePath]'),
        }),
        new ReactRefreshWebpackPlugin(),
        new webpack.DefinePlugin({ 'process.env.NODE_ENV': '"development"' }),
    ].filter(Boolean)
});

