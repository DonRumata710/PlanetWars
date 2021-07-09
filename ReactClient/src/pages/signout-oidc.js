import React, { useEffect } from 'react'
import { signoutRedirectCallback } from '../services/userService'
import { useHistory } from 'react-router-dom'

function SignoutOidc() {
  const history = useHistory()
  useEffect(() => {
    async function signoutAsync() {
      await signoutRedirectCallback()
      history.push('/')
    }
    signoutAsync()
  }, [history])

  return (
    <div>
      Never should have come here.
    </div>
  )
}

export default SignoutOidc
