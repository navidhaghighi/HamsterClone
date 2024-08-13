import mysql from 'mysql2/promise';

async function updateUserProfit(userId, cardId) {
    const connection = await mysql.createConnection({
        host: "localhost",
        user: "root",
        password: "1234",
        database: "hamster_data"
    });

    const query = `
        UPDATE users u
        JOIN cards c ON u.id = 1
        SET u.profit = u.profit + c.profit
        WHERE u.id = 1 AND c.id = 1;
    `;

    try {
        const [result] = await connection.execute(query, [userId, cardId]);
        await connection.end();

        if (result.affectedRows > 0) {
            console.log('User profit updated successfully.');
            return true; // Resolve the promise as true
        } else {
            console.log('No rows affected, check userId and cardId.');
            return false; // Resolve the promise as false if no rows were updated
        }
    } catch (error) {
        await connection.end();
        console.error('Error updating user profit:', error.message);
        throw error; // Reject the promise with the error
    }
}

// Example usage:
updateUserProfit(1, 123)
    .then(result => {
        if (result) {
            console.log('Operation successful.');
        } else {
            console.log('Operation failed.');
        }
    })
    .catch(error => {
        console.error('Error:', error.message);
    });