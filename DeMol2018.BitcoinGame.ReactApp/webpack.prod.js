const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const TerserPlugin = require('terser-webpack-plugin');
const CssMinimizerPlugin = require("css-minimizer-webpack-plugin");

const common = require("./webpack.common.js");
const { merge } = require("webpack-merge");
const webpack = require("webpack");
const ReactRefreshTypeScript = require("react-refresh-typescript");

module.exports = merge(common, {
    mode: "production",
    module: {
        rules: [
            { test: /\.css$/, use: [ MiniCssExtractPlugin.loader, 'css-loader' ]},
            { test: /\.tsx?$/, include: /ClientApp/, use: [{
                loader: require.resolve('ts-loader'),
                options: {
                    transpileOnly: true,
                },
            }], },
            { test: /\.(png|jpg|jpeg|gif|svg|ttf)$/, type: "asset/resource" },
        ]
    },
    optimization: {
        minimizer: [new TerserPlugin(), new CssMinimizerPlugin()]
    },
    plugins: [
        // Plugins that apply in production builds only
        new TerserPlugin({
            parallel: 2
        }),
        new webpack.DefinePlugin({ 'process.env.NODE_ENV': '"production"' }),
    ],
});
