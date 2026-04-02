import type { Config } from 'tailwindcss'

export default {
  content: [
    './components/home-v2/**/*.{js,vue,ts}',
    './pages/home-v2.vue',
  ],
  important: '.v2-page',
  corePlugins: {
    preflight: false,
  },
  theme: {
    extend: {
      colors: {
        teal: {
          DEFAULT: '#2F7785',
          dark: '#225F6B',
        },
        lime: {
          DEFAULT: '#98C73A',
        },
        'off-white': '#F4F4F5',
        gold: '#F5C518',
      },
      fontFamily: {
        sans: ['Jost', 'system-ui', 'sans-serif'],
      },
    },
  },
  plugins: [],
} satisfies Config
