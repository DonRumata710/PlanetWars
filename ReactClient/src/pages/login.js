import React from 'react'
import { login } from '../services/userService'
import { Redirect } from 'react-router-dom'
import { useSelector } from 'react-redux'
import Page from '../components/page'

function Login() {
  const user = useSelector(state => state.auth.user)

  return (
    (user) ?
      (<Redirect to={'/'} />)
      :
      (
        <Page>
          <p>Welcome to PlanetWar server.</p>
          <p>Please, login to continue</p>
          <button onClick={() => login()}>Login</button>
        </Page>
      )
  )
}

export default Login
