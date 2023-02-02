
let users = [];

function getUsers() {
    
    const token=sessionStorage.getItem("token");
    fetch('/User', {
        method: 'GET',
        headers: {
            'Authorization':'Bearer '+token,
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
    })
        .then(response => response.json())
        .then(data => _displayUsers(data))
        .catch(error => console.error('Unable to get items.', error));
}

function _displayUsers(data) {
    const tBody = document.getElementById('users');
    tBody.innerHTML = '';
    const button = document.createElement('button');

    data.forEach(item => {
        let isDoneCheckbox = document.createElement('input');
        isDoneCheckbox.type = 'checkbox';
        isDoneCheckbox.disabled = true;
        isDoneCheckbox.checked = item.isAdmin;

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteUser(${item.id})`);

        let tr = tBody.insertRow();

        let td0 = tr.insertCell(0);
        td0.appendChild(isDoneCheckbox);

        let td1 = tr.insertCell(1);
        let textNodeId = document.createTextNode(item.id);
        td1.appendChild(textNodeId);

        let td2 = tr.insertCell(2);
        let textNode = document.createTextNode(item.name);
        td2.appendChild(textNode);

        // let td3 = tr.insertCell(2);
        // td3.appendChild(editButton);

        let td3 = tr.insertCell(3);
        td3.appendChild(deleteButton);
    });

    users = data;
}

function addUser() {
    debugger
    const addNameTextbox = document.getElementById('add-name');
    const addPasswordTextbox = document.getElementById('add-password');
    const token=sessionStorage.getItem("token");
debugger
    const item = {
        name: addNameTextbox.value.trim(),
        passwrod: addPasswordTextbox.value.trim(),
        isAdmin: false
    };
    
    fetch('/User', {
            method: 'POST',
            headers: {
                'Authorization':'Bearer '+token,
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(item)
        })
        .then(response => response.json())
        .then(() => {
            getUsers();
            addNameTextbox.value = '';
            addPasswordTextbox.value='';
        })
        .catch(error => console.error('Unable to add item.', error));
}

function deleteUser(id) {
    const token=sessionStorage.getItem("token");
    id+=""
    fetch(`/User/${id}`, {
            method: 'DELETE',
            headers: {
                'Authorization':'Bearer '+token,
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            }

        })
        .then(() => getUsers())
        .catch(error => console.error('Unable to delete item.', error));
}