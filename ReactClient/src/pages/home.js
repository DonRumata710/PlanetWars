import React from 'react'
import { NavLink } from 'react-router-dom'
import Page from '../components/page'
import { signoutRedirect } from '../services/userService'
import { useSelector } from 'react-redux'

function Home() {
  const user = useSelector(state => state.auth.user)
  function signOut() {
    signoutRedirect()
  }

  return (
    <Page>
      <NavLink to="/new-session"><h1>Start new Game</h1></NavLink>
      <NavLink to="/sessions"><h1>Join to others</h1></NavLink>
      <NavLink to="/user/" component={user.profile.given_name}><h1>User information</h1></NavLink>
      <button className="button button-clear" onClick={() => signOut()}>Sign Out</button>
    </Page>
  )
}

export default Home
