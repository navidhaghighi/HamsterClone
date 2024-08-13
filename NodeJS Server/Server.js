//#region declarations
import mysql from 'mysql2/promise';

//const wss = new WebSocket('ws://localhost:8080');
// create application/json parser
import bodyParser  from'body-parser';
import { WebSocketServer } from "ws";

import http from 'http';
import express, { json } from'express';
const app = express();
const port = 3000;
const server = http.createServer(app);

var jsonParser = bodyParser.json()
 
// create application/x-www-form-urlencoded parser
var urlencodedParser = bodyParser.urlencoded({ extended: false })

console.log('before running query');
const wss = new WebSocketServer({ noServer: true });

 //#endregion
//#region webSocket
wss.on('connection', (ws) => {
console.log('websocket connected');
//if user id received , start counting profit


wss.on('message', (message) => {
  console.log("receiveed this message "+ message);
  const deserializedMsg = JSON.parse(message);
  let x = message.x;
  switch(deserializedMsg.messageType) {
    case "UserData":
      let userData =  deserializedMsg.user;
      sendWelcomeBackMessage(userData,ws);
      getUserData(userData.id).then(data=>{
        profitTimer(data.id,ws);
      });
      case "ConnectionClose":
        let usr =  deserializedMsg.user;
        setLastConnectionDate(usr.id);
        break;
      default:
        break;
    }

});
wss.on('close',(message)=>
{
  console.log('closed ');
});
  
});
//#endregion
//#region  functions




function setLastConnectionDate(userId)
{
  console.log('set latt ciesda '+userId);
  var updateQuery =`UPDATE users SET last_connection=`+Math.floor(new Date().getTime() / 1000)+ ` WHERE id=`+userId+`;`;
  con.query(updateQuery, function (err, updateResults) {

  });

}

function sendWelcomeBackMessage(userData,wsConnection)
{
  let now = Math.floor(new Date().getTime() / 1000);
  let lastConnection =userData.last_connection;
  let timeSpent = now - lastConnection;
  //TODO: limit it by 3 hours
  let coinBalance = userData.coin_balance;

  let newCoins =coinBalance +  (userData.profitPerHour/3600)*timeSpent;
  wsConnection.send(JSON.stringify({
    messageType:'welcomeBack',
    minedCoins: newCoins
}));
}

//increase profit and inform user every second
function profitTimer(userId,wsConnection)
{
  let newCoin =0;
  let intervalId =  setInterval(function(id,connection){ 

    if(connection.readyState == 3)
    {
      clearInterval(intervalId);
      return;
    }
    getUserData(id).then(userData =>{
      newCoin = Math.ceil( userData.coin_balance+( userData.profit/3600/*because an hour is 3600 seconds*/));


      setCoinBalanceToDB(newCoin,id).then(new_balance=>{
        connection.send(JSON.stringify({
          messageType:'coinsUpdate',
          coin: new_balance
      }));
      });
  });
  },1000,userId,wsConnection) 
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
      console.log('returing result of this query  ' + query + " is this : " + JSON.stringify( result));
      return result;
  } catch (error) {
      await connection.end();
      console.error('Error updating user profit:', error.message);
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
  runQueryOnDB(`SELECT * FROM users WHERE id=`+req.body.userId)
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
  });

});

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
  let userId = req.body.userId;
  let cardId= req.body.cardId;
  runQueryOnDB(`START TRANSACTION;

UPDATE usercards
SET current_level = current_level + 1
WHERE user_id =`+userId+` AND card_id =`+cardId+`;

UPDATE users
SET profit = profit + (
    SELECT current_level * c.profit
    FROM usercards uc
    JOIN cards c ON uc.card_id = c.card_id
    WHERE uc.user_id =`+userId+` AND uc.card_id =`+cardId+`
)
WHERE user_id =`+userId+`;

COMMIT;`)
.then(results => res.end('true'))
  .catch(error => console.error("this is the error of the func " + error));
});

app.post('/buyCard',urlencodedParser,(req,res)=>{
  let userId = req.body.userId;
  let cardId= req.body.cardId;
  runQueryOnDB(`START TRANSACTION;

INSERT INTO usercards (user_id, card_id)
VALUES (`+userId+`,`+cardId+`);

UPDATE users
SET profit = profit + (
    SELECT profit
    FROM cards
    WHERE card_id =`+cardId+`
)
WHERE user_id =`+userId+`;

COMMIT;`)
    .then(results => res.end('true'))
      .catch(error => console.error("this is the error of the func " + error));
});





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