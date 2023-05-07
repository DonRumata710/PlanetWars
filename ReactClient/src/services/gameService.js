import axios from 'axios'

var address = null;
var sessionController = address + '/game';


function setGameServiceAddress(newAddress)
{
    address = newAddress;
    sessionController = address + '/game';
}

async function getSessionState(id) {
    while (address === null)
    {
        const sleep = ms => new Promise(r => setTimeout(r, ms));
        await sleep(100);
    }

    const response = await axios.get(sessionController, { params: { id } });
    return response.data;
}

export {
    setGameServiceAddress,
    getSessionState
}
