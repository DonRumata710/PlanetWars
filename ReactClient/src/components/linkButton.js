import React from 'react'
import PropTypes from 'prop-types'
import { withRouter } from 'react-router'

const LinkButton = (props) => {
  const {
    history,
    location,
    match,
    staticContext,
    to,
    onClick,
    // ⬆ filtering out props that `button` doesn’t know what to do with.
    ...rest
  } = props
  return (
    <button
      {...rest} // `children` is just another prop!
      onClick={async (event) => {
        var link = to
        if (onClick)
        {
          if (link === undefined)
          {
            var newLink = await onClick(event)
            if (newLink)
              link = newLink
          }
          else
          {
            onClick(event)
          }
        }
        history.push(link)
      }}
    />
  )
}

LinkButton.propTypes = {
  children: PropTypes.node.isRequired
}

export default withRouter(LinkButton)
