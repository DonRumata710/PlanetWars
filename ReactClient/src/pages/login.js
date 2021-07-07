import React from 'react'
import { signinRedirect } from '../services/userService'
import { Redirect } from 'react-router-dom'
import { useSelector } from 'react-redux'
import Header from '../components/header'

function Login() {
  const user = useSelector(state => state.auth.user)

  function login() {
    signinRedirect()
  }

  return (
    (user) ?
      (<Redirect to={'/'} />)
      :
      (
        <div>
          <Header />
          <p>Welcome to PlanetWar server.</p>
          <p>Please, login to continue</p>
          <button onClick={() => login()}>Login</button>
        </div>
      )
  )
}

export default Login
