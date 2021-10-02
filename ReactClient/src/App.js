import React, { useEffect } from 'react'
import { BrowserRouter, Route, Switch } from 'react-router-dom'
import SigninOidc from './pages/signin-oidc'
import SignoutOidc from './pages/signout-oidc'
import Home from './pages/home'
import Login from './pages/login'
import GamePage from './pages/game'
import UserPage from './pages/user'
import NewSession from './pages/newSession'
import Sessions from './pages/sessions'
import './App.css'
import userManager, { loadUserFromStorage } from './services/userService'
import AuthProvider from './utils/authProvider'
import ProtectedRoute from './components/protectedRoute'
import { Provider } from 'react-redux';
import store from './store'


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
            <ProtectedRoute path='/new-session' component={NewSession} />
            <ProtectedRoute path='/sessions' component={Sessions} />
            <ProtectedRoute path='/game/:sessionId?' component={GamePage} />
            <ProtectedRoute path='/user/:userId?' component={UserPage} />
          </Switch>
        </BrowserRouter>
      </AuthProvider>
    </Provider>
  );
}

export default App;
