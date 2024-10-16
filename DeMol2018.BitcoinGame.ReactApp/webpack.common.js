const path = require('path');
const webpack = require('webpack');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const HtmlWebpackPlugin = require("html-webpack-plugin");

const clientBundleOutputDir = './public/dist';

module.exports = {
    entry: {
        'main-client': [
            'bootstrap/dist/css/bootstrap.css',
            './ClientApp/ClientApp.tsx',
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
    ],
    stats: { modules: false },
    resolve: { extensions: ['.js', '.jsx', '.ts', '.tsx'] },
};
