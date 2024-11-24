/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./Components/**/*.razor"],
  theme: {
    extend: {},
  },
  plugins: [
    require("@tailwindcss/typography"),
    require("daisyui"),
  ],
  daisyui: {
    themes: ["black"],
  },
  corePlugins: {
    aspectRatio: false,
  },
};
