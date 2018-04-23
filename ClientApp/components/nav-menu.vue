<template>
    <div class="main-nav">
        <div class="navbar navbar-inverse">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle" v-on:click="toggleCollapsed">
                    <span class="sr-only">Toggle navigation</span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                <a class="navbar-brand" href="/">iEnge Automation</a>
            </div>
            <ul class="nav navbar-nav navbar-right">
                <li>
                    <button class="btn btn-danger log" @click="handleLogout()">Log out </button>
                    <button class="btn btn-info log" @click="handleLogin()">Log In</button>
                </li>
            </ul>
            <div class="clearfix"></div>
            <transition name="slide">
                <div class="navbar-collapse collapse in" v-show="!collapsed">
                    <ul class="nav navbar-nav">
                        <li v-for="route in routes">
                            <!-- TODO: highlight active link -->
                            <router-link :to="route.path">
                                <span :class="route.style"></span> {{ route.display }}
                            </router-link>
                        </li>
                    </ul>
                </div>
            </transition>
        </div>
    </div>
</template>

<script>
    import { routes } from '../routes'
    import { isLoggedIn, login, logout } from '../utils/auth';

    export default {
    name: 'app-nav',
    data() {
        return {
            routes,
            collapsed : true
        }
    },
    methods: {
        toggleCollapsed: function(event){
            this.collapsed = !this.collapsed;
        },
        handleLogin() {
            login();
        },
        handleLogout() {
            logout();
        },
        isLoggedIn() {
            return isLoggedIn();
        }
    }
}
</script>

<style>
.slide-enter-active, .slide-leave-active {
  transition: max-height .35s
}
.slide-enter, .slide-leave-to {
  max-height: 0px;
}

.slide-enter-to, .slide-leave {
  max-height: 20em;
}

.navbar-right {
    margin-right: 0px !important
}

.log {
    margin: 5px 10px 0 0;
}
</style>
