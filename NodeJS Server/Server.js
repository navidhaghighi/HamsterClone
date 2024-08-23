//#region declarations
const mysql =  require( 'mysql2/promise');

//const wss = new WebSocket('ws://localhost:8080');
// create application/json parser
const bodyParser = require ('body-parser');
const http = require('http');
const express =require('express');
const app = express();
const port = 3000;
const server = http.createServer(app);
// Create an empty dictionary to store WebSocket connections for each connected socket
const intervals = new Map();
var jsonParser = bodyParser.json()
 
// create application/x-www-form-urlencoded parser
var urlencodedParser = bodyParser.urlencoded({ extended: false })

console.log('before running query');

 //#endregion
//#region webSocket

const WebSocket = require('ws');
const wss = new WebSocket.Server({ port: 8080 });

wss.on('connection', (ws) => {
console.log('websocket connected');
//if user id received , start counting profit

ws.on('message', (message) => {
  var intervalId = 0;
  console.log("receiveed this message "+ message);
  const deserializedMsg = JSON.parse(message);
  let x = message.x;
  let userId =0;
  switch(deserializedMsg.messageType) {
    case "UserData":
      let userData =  deserializedMsg.user;
      userId = userData.id;
      intervalId = startUpdating(userData.id,ws);
      case "ConnectionClose":
        let usr =  deserializedMsg.user;
        userId = usr.id;
        break;
      default:
        break;
    }
    ws.on('close',(message)=>
      {
       clearInterval(intervalId);
       setLastConnectionDate(userId);
        console.log('closed ');
      });
});

});
//#endregion
//#region  functions




function setLastConnectionDate(userId)
{
  console.log('set latt ciesda '+userId);
  var updateQuery =`UPDATE users SET last_connection=`+Math.floor(new Date().getTime() / 1000)+ ` WHERE id=`+userId+`;`;
  runQueryOnDB(updateQuery);

}

function getInactiveTimeCoinsAmount(userData,wsConnection)
{
  let now = Math.floor(new Date().getTime() / 1000);
  let lastConnection =userData.last_connection;
  let timeSpent = now - lastConnection;
  //TODO: limit it by 3 hours
  let addedCoins = (userData.profit/3600)*timeSpent;
  return addedCoins;
}



async function runQueryOnDB(query) {
  const connection = await mysql.createConnection({
    host: "localhost",
    user: "root",
    password: "1234",
    database: "hamster_data"
  });

  try {
      const [result] = await connection.execute(query);
      await connection.end();
      return result;
  } catch (error) {
      await connection.end();
      console.error('This query '+ query + ' returned this error '+  error.message);
      throw error; // Reject the promise with the error
  }
}






//#endregion

//#region DB connection


/* //old mysql
con.connect(function (err) {
   if (err) throw err;
   console.log("DB Connected!");

});*/
//#endregion


//#region requests




app.get('/', (req, res) => {
  res.send('Hello World!');
});

app.post('/login',urlencodedParser, (req, res) => {
  console.log(('logining '));
  sendLoginMessage(req.body.userId,res);
  /*
  runQueryOnDB()
  .then(result => {
      if (result) {
        console.log('ending '+JSON.stringify(result) );
        res.end(JSON.stringify(result[0]));
      } else {
        res.end("failed");
      }
  })
  .catch(error => {
      console.error('Error:', error.message);
  });*/
});

async function sendLoginMessage(userId,response)
{
  let getUserDataQuery  = `SELECT * FROM users WHERE id=`+userId;
  const userDataResult = await runQueryOnDB(getUserDataQuery);
  let inactiveCoins =  getInactiveTimeCoinsAmount(userDataResult[0]);
  //add coins to coin balance 
  const addCoinsQuery = `UPDATE users
SET coin_balance = coin_balance + ${inactiveCoins}
WHERE id =`+userId+`;`;
const addCoinsResult= await runQueryOnDB(addCoinsQuery);
const userDataAfterUpdate= await runQueryOnDB(getUserDataQuery);
  response.send({user:userDataAfterUpdate[0] ,coinsReceived : inactiveCoins });
}

app.post('/tapped',urlencodedParser, (req, res) => {
  console.log('tapped called ');
  let userId = req.body.userId.toString();
  let updateQuery =`UPDATE users
SET coin_balance = coin_balance + earn_per_tap
WHERE id =`+userId+`;`;

let selectQuery =`SELECT * FROM users
WHERE id =`+userId+`;`;
runQueryOnDB(updateQuery)
.then(updateResult=> runQueryOnDB(selectQuery).then(selectResult=>res.send(selectResult[0])))
  .catch(error => console.error("this is the error of the func " + error));
});

app.post('/getUserCards',urlencodedParser, (req, res) => {
  let userId = req.body.userId;
  let query =`SELECT *
FROM usercards
WHERE user_id =`+userId.toString()+`;`;
runQueryOnDB(query)
.then(results => res.send(  {userCards: results}))
  .catch(error => console.error("this is the error of the func " + error));
});




app.get('/createNewUser', (req, res) => {
  console.log('creating new user')
let insertQuery =`INSERT INTO users (id, name) VALUES (DEFAULT, DEFAULT)`;
let selectQuery = `SELECT *
FROM users
ORDER BY id DESC
LIMIT 1;`
runQueryOnDB(insertQuery)
.then(insertResult => runQueryOnDB(selectQuery)
.then(selectResult =>
  res.send({user: selectResult[0]}))
)
})
app.post('/upgradeCard',urlencodedParser,(req,res)=>{
  console.log('upgrading');
  let userId = req.body.userId;
  let cardId= req.body.cardId;
  upgradeCardAndUserProfit(userId,cardId,res);
 /* console.log('upgrade card functions card id '+ cardId+ ' userid is  '+ userId);
  let getCardQuery = `SELECT * FROM cards WHERE id=`+cardId + `;`;
let secondUpdateQuery = `
UPDATE users
SET profit = profit + (
    SELECT current_level * c.profit
    FROM usercards uc
    JOIN cards c ON uc.card_id = c.id
    WHERE uc.user_id =`+userId+` AND uc.card_id =`+cardId+`
    LIMIT 1
)
WHERE id =`+userId+`;`;


runQueryOnDB(getCardQuery)
.then(card=>
  runQueryOnDB(`UPDATE usercards
SET current_level = current_level + 1 ,profit =`+card[0].profit+` 
WHERE user_id =`+userId+` AND card_id =`+cardId+`;`))
.then(firstUpdateResult => runQueryOnDB(secondUpdateQuery)
.then(secondUpdateResult=>runQueryOnDB(`UPDATE usercards
  SET profit = profit + (profit*)`)))
  .catch(error => console.error("this is the error of the func " + error));*/
});



async function upgradeCardAndUserProfit(userId, cardId,response) {
  try {
      // First query: Get the card details
      const cardQuery = `SELECT * FROM cards WHERE id=${cardId};`;
      const userCardQuery = `SELECT * FROM usercards WHERE user_id=${userId} AND card_id=${cardId};`;
      const selectUserQuery = 'SELECT * FROM users WHERE id='+userId+';';
      let getUserRankQuery = `SELECT * FROM userranks WHERE user_id=${userId}`;
      const cardResult = await runQueryOnDB(cardQuery);
      const userCardResult = await runQueryOnDB(userCardQuery);

      if (cardResult.length === 0) {
          throw new Error('Card not found');
      }
      const userData = await runQueryOnDB(selectUserQuery);
      const userCoins = userCoinsObject[0].coin_balance;
      const card = cardResult[0]; // Assuming there's exactly one card with the given id
      const userCard = userCardResult[0];
      //if user's coins are not enough , send failure as response 
      if(userCoins< userCard.cost)
      {
        response.send({responseResult:'failure' ,errorMessage : 'Not enough coins' });
        return null;
      }
      const profit_multiplier = card.profit_multiplier;
      const cost_multiplier = card.cost_multiplier;
      // Second query: Update the user's profit
      const updateUserProfitQuery = `
          UPDATE users
          SET profit = profit + (
              SELECT current_level * c.profit
              FROM usercards uc
              JOIN cards c ON uc.card_id = c.id
              WHERE uc.user_id = ${userId} AND uc.card_id = ${cardId}
              LIMIT 1
          ) , coin_balance = (coin_balance - ${(userCard.cost * card.cost_multiplier)}),
           coins_to_level_up
          WHERE id = ${userId};
      `;
      await runQueryOnDB(updateUserProfitQuery);

      // Third query: Update the usercards table using the card's multiplier
      const updateUserCardsQuery = `
          UPDATE usercards
          SET profit =  profit + (${card.profit} * ${profit_multiplier}),current_level = current_level+1,
          cost=cost+(${card.cost*cost_multiplier}) 
          WHERE user_id = ${userId} AND card_id = ${cardId};
      `;
      await runQueryOnDB(updateUserCardsQuery);
      let finalUserCards = runQueryOnDB('SELECT * FROM usercards WHERE user_id='+ userId);
      console.log('sending this as final as response '+ finalUserCards);
      response.send(finalUserCards);
      console.log('Card and user profits updated successfully.');

  } catch (error) {
      console.error('Error upgrading card and user profit:', error.message);
  }
}



app.post('/buyCard',urlencodedParser,(req,res)=>{
  console.log('buying a card ');
  let userId = req.body.userId;
  let cardId= req.body.cardId;
  runBuyCardQueries(userId,cardId,res);

});

function startUpdating(userId,connection) {
  let intervalId =  setInterval(() => {
    console.log('inside inter vald');
    let giveCoinQuery =`UPDATE users SET coin_balance= (coin_balance+profit/3600) WHERE id = ${userId}`; 
    let getUserDataQuery = `SELECT coin_balance FROM users WHERE id=${userId}`;
    runQueryOnDB(giveCoinQuery)
    .then(result=>runQueryOnDB(getUserDataQuery))
    .then(coinBalance=>
    connection.send(JSON.stringify({
      messageType:'coinsUpdate',
      coin: coinBalance[0].coin_balance
  })));
  }, 1000); // Every second
  return intervalId;
}



async function runBuyCardQueries(userId, cardId,response) {
  try {
      // First query: Get the card details
      const cardQuery = `SELECT * FROM cards WHERE id=${cardId};`;
      const cardResult = await runQueryOnDB(cardQuery);

      if (cardResult.length === 0) {
          throw new Error('Card not found');
      }
      const selectUserQuery = 'SELECT coin_balance FROM users WHERE id='+userId+';';
      const userCoinsObject = await runQueryOnDB(selectUserQuery);
      const userCoins = userCoinsObject[0].coin_balance;
      const card = cardResult[0]; // Assuming there's exactly one card with the given id
      //if user's coins are not enough , send failure as response 
      if(userCoins< card.cost)
      {
        response.send({responseResult:'failure' ,errorMessage : 'Not enough coins' });
        return null;
      }
      let addCardQuery = `INSERT INTO usercards (user_id, card_id,profit)
      VALUES (`+userId+`,`+cardId+`,`+card.profit+`);`
      await runQueryOnDB(addCardQuery);
      let decreaseCoinsQuery = `UPDATE users SET coin_balance= (coin_balance - ${card.cost}) ;`;
      await runQueryOnDB(decreaseCoinsQuery);


      // Second query: Update the user's profit
      const updateUserProfitQuery = `
          UPDATE users
          SET profit = ${card.profit}
          WHERE id = ${userId};
      `;
      await runQueryOnDB(updateUserProfitQuery);
      response.send({responseResult:'success'});
      console.log('Card and user profits updated successfully.');

  } catch (error) {
      console.error('Error buying card ', error.message);
  }
}





app.get('/getMiningCards', (req, res) => {
  console.log('getmining cards was calle ');
  let selectQuery = 'SELECT * FROM cards;'
  runQueryOnDB(selectQuery)
  .then(results => res.send( {cards:results}))
  .catch(error => console.error("this is the error of the func " + error));
});

app.listen(port, () => {
  console.log(`Example app listening at http://localhost:${port}`);
});

app.use('/images', express.static('images'));
//#endregion