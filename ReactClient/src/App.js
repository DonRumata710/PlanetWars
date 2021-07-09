import React, { useEffect } from 'react'
import { BrowserRouter, Route, Switch } from 'react-router-dom'
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
    loadUserFromStorage(store)
  }, [])
  
  return (
    <Provider store={store}>
      <AuthProvider userManager={userManager} store={store}>
        <BrowserRouter>
          <Switch>
            <Route path='/login' component={Login} />
            <Route path="/signout-oidc" component={SignoutOidc} />
            <Route path="/signin-oidc" component={SigninOidc} />
            <ProtectedRoute exact path='/' component={Home} />
            <ProtectedRoute path='/game/:sessionId?' component={GamePage} />
            <ProtectedRoute path='/user/:userId?' component={UserPage} />
          </Switch>
        </BrowserRouter>
      </AuthProvider>
    </Provider>
  );
}

export default App;
