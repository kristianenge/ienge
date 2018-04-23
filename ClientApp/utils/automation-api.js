import axios from 'axios';

const BASE_URL = 'http://localhost:54187';

export { getAutomationProjects };

function getAutomationProjects() {
    const url = `${BASE_URL}/api/automation/automationprojects`;
    return axios.get(url).then(response => response.data);
}

