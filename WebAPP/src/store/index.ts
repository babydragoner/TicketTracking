import { defineStore } from 'pinia'

export const useTodoStore = defineStore({
  id: 'todo',
  state: () => ({ count: 0, title: "Cook noodles", done:false })
})

// import type { App } from 'vue';
// import { createPinia } from 'pinia';

// const store = createPinia();

// export function setupStore(app: App<Element>) {
//   app.use(store);
// }

// export { store };