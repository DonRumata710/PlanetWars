import React, { useEffect } from 'react'
import { signinRedirectCallback } from '../services/userService'
import { useHistory } from 'react-router-dom'

function SigninOidc() {
  const history = useHistory()
  useEffect(() => {
    async function signinAsync() {
      await signinRedirectCallback()
      history.push('/')
    }
    signinAsync()
  }, [history])

  return (
    <div>
      Never should have come here.
    </div>
  )
}

export default SigninOidc
