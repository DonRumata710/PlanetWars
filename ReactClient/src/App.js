import React, { Component, useEffect } from 'react'
import { BrowserRouter, Route } from 'react-router-dom'
import Home from './pages/home'
import Login from './pages/login'
import GamePage from './pages/game'
import UserPage from './pages/user'
import './App.css'
import userManager, { loadUserFromStorage } from './services/userService'
import AuthProvider from './utils/authProvider'
import ProtectedRoute from './utils/protectedRoute'
import { Provider } from 'react-redux';
import store from './store'
import SigninOidc from './pages/signin-oidc'
import SignoutOidc from './pages/signout-oidc'


function App() {
  useEffect(() => {
    // fetch current user from cookies
    loadUserFromStorage(store)
  }, [])
  
  return (
    <div>
      <Provider store={store}>
        <AuthProvider userManager={userManager} store={store}>
          <BrowserRouter>
            <ProtectedRoute exact path='/' component={Home} />
            <Route path='/login' component={Login} />
            <Route path='/game/:sessionId?' component={GamePage} />
            <Route path='/user/:userId?' component={UserPage} />
            <Route path="/signout-oidc" component={SignoutOidc} />
            <Route path="/signin-oidc" component={SigninOidc} />
          </BrowserRouter>
        </AuthProvider>
      </Provider>
    </div>
  );
}

export default App;
