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
            { test: /\.(png|jpg|jpeg|gif|svg|ttf)$/, type: "asset/resource" },
        ]
    },
    plugins: [
        new webpack.EnvironmentPlugin({
            NODE_ENV: "development",
        }),
        new webpack.SourceMapDevToolPlugin({
            filename: '[file].map',
            moduleFilenameTemplate: path.relative(clientBundleOutputDir, '[resourcePath]'),
        }),
        new ReactRefreshWebpackPlugin(),
    ].filter(Boolean)
});

