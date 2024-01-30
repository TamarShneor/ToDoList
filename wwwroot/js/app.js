const uri = '/Task';


function login() {
    debugger
    let status = false;
    const name = document.getElementById('name');
    const password = document.getElementById('password');

    const user = {
        Id: 0,
        Name: name.value.trim(),
        Password: password.value.trim(),
        IsAdmin: false
    }
    fetch('https://localhost:7135/User/Login', {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(user)
    })
        .then(response => response.json())
        .then((token) => {
            token += "";
            if (!(token.includes("object"))) {
                sessionStorage.setItem("token", token);
                name.value = '';
                password.value = '';
                location.href = "todo.html";
            }
            else {

                alert("Not Found User")
            }

        })
        .catch(error => console.error('Unable to add item.', error));
}


function getTasks() {
    debugger
    const token = sessionStorage.getItem("token");
    fetch('/Task', {
        method: 'GET',
        headers: {
            'Authorization': 'Bearer ' + token,
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
    })
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}


function _displayItems(data) {
    const tBody = document.getElementById('tasks');
    tBody.innerHTML = '';

    _displayCount(data.length);

    const button = document.createElement('button');

    data.forEach(item => {
        let isDoneCheckbox = document.createElement('input');
        isDoneCheckbox.type = 'checkbox';
        isDoneCheckbox.disabled = true;
        isDoneCheckbox.checked = item.isDone;

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        td1.appendChild(isDoneCheckbox);

        let td2 = tr.insertCell(1);
        let textNode = document.createTextNode(item.name);
        td2.appendChild(textNode);

        let td3 = tr.insertCell(2);
        td3.appendChild(editButton);

        let td4 = tr.insertCell(3);
        td4.appendChild(deleteButton);
    });

    tasks = data;
}

function addTask() {
    debugger
    const addNameTextbox = document.getElementById('add-name');
    const token = sessionStorage.getItem("token");

    const item = {
        isDone: false,
        name: addNameTextbox.value.trim()
    };

    fetch('/Task', {
        method: 'POST',
        headers: {
            'Authorization': 'Bearer ' + token,
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(response => response.json())
        .then(() => {
            getTasks();
            addNameTextbox.value = '';
        })
        .catch(error => console.error('Unable to add item.', error));
}

function updateItem() {
    const itemId = document.getElementById('edit-id').value;
    const token = sessionStorage.getItem("token");
    const item = {
        id: parseInt(itemId, 10),
        isdone: document.getElementById('edit-isDone').checked,
        name: document.getElementById('edit-name').value.trim(),
        userId: 0
    };
    fetch(`${uri}/${itemId}`, {
        method: 'PUT',
        headers: {
            'Authorization': 'Bearer ' + token,
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(() => getTasks())
        .catch(error => console.error('Unable to update item.', error));

    closeInput();
    return false;
}

function deleteItem(id) {
    const token = sessionStorage.getItem("token");
    id += ""
    fetch(`${uri}/${id}`, {
        method: 'DELETE',
        headers: {
            'Authorization': 'Bearer ' + token,
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        }

    })
        .then(() => getTasks())
        .catch(error => console.error('Unable to delete item.', error));
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function displayEditForm(id) {
    const item = tasks.find(item => item.id === id);

    document.getElementById('edit-name').value = item.name;
    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-isDone').checked = item.isDone;
    document.getElementById('editForm').style.display = 'block';
}

function _displayCount(itemCount) {
    const name = (itemCount <= 1) ? 'task' : 'task kinds';

    document.getElementById('counter').innerText = `${itemCount} ${name}`;
}



/////////////////////////////////
function getMyUser() {
    debugger
    const token = sessionStorage.getItem("token");
    fetch('/User/0', {
        method: 'GET',
        headers: {
            'Authorization': 'Bearer ' + token,
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
    })
        .then(response => response.json())
        .then(data => myUser(data))
        .catch(error => console.error('Unable to get items.', error));
}

function myUser(user) {
    const userId = document.getElementById('my-user');
    const username = document.createElement('p');
    username.innerHTML = user.name;
    userId.appendChild(username);
    if (user.isAdmin == true) {
        const button = document.createElement('button');
        let setUsers = button.cloneNode(false);
        setUsers.innerText = 'set users';
        setUsers.setAttribute('onclick', `location.href="users.html"`);
        userId.appendChild(setUsers);
    }
}
