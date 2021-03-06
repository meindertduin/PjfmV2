import adapter from '@sveltejs/adapter-node';
import preprocess from 'svelte-preprocess';

/** @type {import('@sveltejs/kit').Config} */
const config = {
    // Consult https://github.com/sveltejs/svelte-preprocess
    // for more information about preprocessors
    preprocess: preprocess({
        scss: {
            prependData: `@import './scss/styles.scss';`,
        }
    }),

    kit: {
        adapter: adapter(),
        vite: () => ({
            server: {
                cors: true,
            }
        })
    },
};

export default config;
