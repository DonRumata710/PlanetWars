import axios from 'axios'

async function getGameFromApi() {
  const response = await axios.get('https://localhost:44323/game');
  return response.data;
}

export {
  getGameFromApi
}
