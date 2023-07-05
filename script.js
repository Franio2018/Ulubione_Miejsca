function userLogin() {
    var userLoginData = {
        name: document.getElementById('UserLoginName').value,
        email: document.getElementById('UserLoginEmail').value,
        password: document.getElementById('UserLoginPassword').value
    };

    $.ajax({
        url: 'https://localhost:7250/UlubioneMiejsca/userLoginData',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(userLoginData),
        success: function (response) {
            document.getElementById('response').textContent = 'User ID: ' + response;
        },
        error: function (xhr, status, error) {
            document.getElementById('response').textContent = 'Error: ' + error;
        }
    });
}

function getUserFavoritePlaces() {
    var userId = document.getElementById('userIdFavPlaces').value;
    var url = 'https://localhost:7250/UlubioneMiejsca?userId=' + userId;

    fetch(url)
        .then(function (response) {
            if (!response.ok) {
                throw new Error('Request failed with status: ' + response.status);
            }
            return response.json();
        })
        .then(function (data) {
            var responseDiv = document.getElementById('response');
            responseDiv.innerHTML = '';

            if (data.length === 0) {
                responseDiv.innerHTML = 'No favorite places found for the user.';
                return;
            }

            var table = document.createElement('table');

            // Create table header
            var headerRow = document.createElement('tr');
            var headers = Object.keys(data[0]);
            for (var i = 0; i < headers.length; i++) {
                var headerCell = document.createElement('th');
                headerCell.textContent = headers[i];
                headerRow.appendChild(headerCell);
            }
            table.appendChild(headerRow);

            // Create table rows
            for (var j = 0; j < data.length; j++) {
                var rowData = data[j];
                var row = document.createElement('tr');
                for (var prop in rowData) {
                    var cell = document.createElement('td');
                    cell.textContent = rowData[prop];
                    row.appendChild(cell);
                }
                table.appendChild(row);
            }

            responseDiv.appendChild(table);
        })
        .catch(function (error) {
            document.getElementById('response').textContent = 'Error: ' + error.message;
        });
}

function addUser() {
    var userData = {
        name: document.getElementById('AddUserName').value,
        surname: document.getElementById('AddUserSurname').value,
        email: document.getElementById('AddUserEmail').value,
        password: document.getElementById('AddUserPassword').value,
        phone: document.getElementById('AddUserPhone').value
    };

    $.ajax({
        url: 'https://localhost:7250/UlubioneMiejsca',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(userData),
        success: function (response) {
            document.getElementById('response').textContent = response;
        },
        error: function (xhr, status, error) {
            document.getElementById('response').textContent = 'Error: ' + error;
        }
    });
}

$(document).ready(function() {
    // Handle form submission
    $('#userProfileForm').submit(function(event) {
      event.preventDefault(); // Prevent the default form submission
  
      // Call the addUserProfile function
      addUserProfile();
    });
  });
  
  function addUserProfile() {
    var userProfileData = {
      userId: document.getElementById('userIdProfile').value,
      // Add other properties here based on your UserProfile class in C#
    };
  
    $.ajax({
      url: 'https://localhost:7250/UlubioneMiejsca/userProfile',
      type: 'POST',
      contentType: 'application/json',
      data: JSON.stringify(userProfileData),
      success: function(response) {
        document.getElementById('response').textContent = response;
      },
      error: function(xhr, status, error) {
        document.getElementById('response').textContent = 'Error: ' + error;
      }
    });
  }