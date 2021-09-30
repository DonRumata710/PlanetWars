import axios from 'axios'


var launchServer = 'https://localhost:44348'
var sessionController = launchServer + '/sessions';
var userController = launchServer + '/user';


async function getSessions() {
  const response = await axios.get(sessionController);
  return response.data;
}

async function joinSession(sessionId) {
  await axios.post(sessionController + '/join',
    { params: { sessionId } }
  );
}

async function createSession(param) {
    const response = await axios.post(sessionController,
      param
    );
    return response.data;
}

async function getDefaultGameParameters() {
  const response = await axios.get(sessionController + '/default');
  return response.data;
}

async function updateSession(sessionId, params) {
  await axios.put(sessionController,
    { sessionId, params }
  );
}

async function startSession(id) {
  await axios.post(sessionController + '/start',
    { params: { id } }
  )
}

async function getUserInfo(username) {
  const response = await axios.get(userController,
    { params: { username } }
  );
  return response.data;
}

export {
    getSessions,
    joinSession,
    createSession,
    getDefaultGameParameters,
    updateSession,
    startSession,
    getUserInfo
}
