import { createStore, compose } from 'redux';
import allReducers from './reducers'

const composeEnhancers = window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ || compose;
const initialState = {}

const store = createStore(
  allReducers,
  initialState,
  composeEnhancers()
)

export default store;
