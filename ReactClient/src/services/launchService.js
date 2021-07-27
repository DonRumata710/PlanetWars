import axios from 'axios'

async function getSessions() {
  const response = await axios.get('https://localhost:44348/sessions');
  return response.data;
}

async function createSession(params) {
    const response = await axios.post('https://localhost:44348/sessions', params);
    return response.data;
}

async function getDefaultGameParameters() {
  const response = await axios.get('https://localhost:44348/sessions/default');
  return response.data;
}

export {
    getSessions,
    createSession,
    getDefaultGameParameters
}
