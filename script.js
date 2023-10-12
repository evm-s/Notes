const saveButton = document.querySelector('#btnSave');
const titleInput = document.querySelector('#title');
const descriptionInput = document.querySelector('#description');
const notesContainer = document.querySelector('#notes__container');
const deleteButton = document.querySelector('#btnDelete');

function cleatForm() {
    titleInput.value = '';
    descriptionInput.value = '';
    deleteButton.classList.add('hidden');
}

function displayNoteInForm(note) {
    titleInput.value = note.title;
    descriptionInput.value = note.description;
    deleteButton.classList.remove('hidden');
    deleteButton.setAttribute('data-id'. note.id);
    saveButton.setAttribute('data-id'. note.id);

}

function getNoteById() {
    fetch(`https://localhost:5214/api/notes/${id}`)
    .then (data => data.json())
    .then(response => displayNoteInForm(response));
}

function populateForm (id) {
    getNoteById(id);
}

function addNote(title, description) {
    const body = {
        title: title,
        description: description,
        isVisible: true
    };

    fetch(`https://localhost:5214/api/notes`, {
        method: 'POST',
        body: JSON.stringify(body),
        headers: {
            "content-type": "application/json"
        }
    })
    .then (data => data.json())
    .then(response => {
        cleatForm()
        getAllNotes();
    });
}


function displayNotes(notes) {
    let allNotes = '';

    notes.forEach(note => {
        const noteElement = `
                                <div class="note" data-id = "${note.id}">
                                    <h3>${note.title}</h3>
                                    <p>${note.description}</p>
                                </div>
                            `;
        allNotes += noteElement;               
    });

    notesContainer.innerHTML = noteElement;

    document.querySelectorAll('.note').forEach(note => {
        note.addEventListener('click', function(e) {
            populateForm(note.dataset.id);
        })
    })
}

function getAllNotes() {
    fetch(`https://localhost:5214/api/notes`)
    .then (data => data.json())
    .then(response => console.log(response));
}

getAllNotes();

function updateNote(id, title, description) {
    const body = {
        title: title,
        description: description,
        isVisible: true
    };

    fetch(`https://localhost:5214/api/notes/${id}`, {
        method: 'PUT',
        body: JSON.stringify(body),
        headers: {
            "content-type": "application/json"
        }
    })
    .then (data => data.json())
    .then(response => {
        cleatForm()
        getAllNotes();
    });
}

saveButton.addEventListener('click', function() {
    const id = saveButton.dataset.id;

    if (id) {
        updateNote (id, titleInput.value, descriptionInput.value)
    } else {
        addNote(titleInput.value, descriptionInput.value);
    }
 });

function deleteNote(id) {
    fetch(`https://localhost:5214/api/notes/${id}`, {
        method: 'DELETE',
        headers: {
            "content-type": "application/json"
        }
    })
    .then(response => {
       clearForm();
       getAllNotes();
    });
}

addButton.addEventListener('click', function () {
    const id = deleteButton.dataset.id;
    deleteNote(id);
})