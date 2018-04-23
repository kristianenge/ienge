import Vue from 'vue'
import axios from 'axios'
import router from './router'
import store from './store'
import Loading from 'components/lib/loading'
import CenterContainer from 'components/lib/center-container'
import { sync } from 'vuex-router-sync'
import App from 'components/App'

Vue.prototype.$http = axios;
Vue.component('loading', Loading)
Vue.component('center-container', CenterContainer)
sync(store, router)

const app = new Vue({
    store,
    router,
    ...App
})

export {
    app,
    router,
    store
    }
