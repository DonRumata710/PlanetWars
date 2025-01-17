import React from 'react'
import { NavLink } from 'react-router-dom'
import Page from '../components/page'
import { useSelector } from 'react-redux'

function Home() {
  const user = useSelector(state => state.auth.user)

  return (
    <Page>
      <NavLink to="/new-session"><h1>Start new Game</h1></NavLink>
      <NavLink to="/sessions"><h1>Join to others</h1></NavLink>
      <NavLink to={"/user/" + user.profile.name}><h1>User information</h1></NavLink>
    </Page>
  )
}

export default Home
