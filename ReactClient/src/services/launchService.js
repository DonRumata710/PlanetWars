import axios from 'axios'


var launchServer = 'https://localhost:44348'
var sessionController = launchServer + '/sessions';
var userController = launchServer + '/user';
var ApiUrl = launchServer + '/api';


async function getSessions() {
  const response = await axios.get(sessionController);
  return response.data;
}

async function getSession(id) {
  const response = await axios.get(sessionController, { params: { id } });
  return response.data;
}

async function joinSession(sessionId) {
  await axios.get(sessionController + '/join', { params: { sessionId } });
}

async function leaveSession() {
  await axios.get(sessionController + '/leave');
}

async function createSession(param) {
    const response = await axios.post(sessionController, param);
    return response.data;
}

async function getDefaultGameParameters() {
  const response = await axios.get(sessionController + '/default');
  return response.data;
}

async function updateSession(sessionId, params) {
  await axios.put(sessionController, { sessionId, params });
}

async function startSession(id) {
  const response = await axios.get(sessionController + '/start', { params: { id } })
  return response.data;
}

async function getUserName(id) {
  const response = await axios.get(userController + "/" + id + "/name", { params: { id } });
  return response.data;
}

async function getUserInfo(username) {
  const response = await axios.get(userController, { params: { username } });
  return response.data;
}

export {
    getSessions,
    getSession,
    joinSession,
    leaveSession,
    createSession,
    getDefaultGameParameters,
    updateSession,
    startSession,
    getUserName,
    getUserInfo,
    ApiUrl
}
