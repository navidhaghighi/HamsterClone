//#region declarations
const WebSocket = require('ws');
const wss = new WebSocket.Server({ port: 8080 });
// create application/json parser
var bodyParser = require('body-parser')
var jsonParser = bodyParser.json()
 
// create application/x-www-form-urlencoded parser
var urlencodedParser = bodyParser.urlencoded({ extended: false })
const express = require('express');
const app = express();
const port = 3000;



 
 //#endregion
//#region webSocket
 wss.on('connection', (ws) => {
console.log('websocket connected');
//if user id received , start counting profit


ws.on('message', (message) => {
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
ws.on('close',(message)=>
{
  console.log('closed ');
});
  
});
//#endregion
//#region  functions
function buyMiningCard(cardId,userId)
{
  var insertQuery =`INSERT INTO usercards(card_id,user_id) VALUES(`+ cardId+`,`+userId+`);`;
  con.query(insertQuery, function (err, updateResults) {

  });
}

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
  setInterval(function(id,connection){ 


    getUserData(id).then(userData =>{
      console.log('user data . coin before adding '+ userData.coin_balance);
      newCoin = Math.ceil( userData.coin_balance+( userData.profit/3600/*because an hour is 3600 seconds*/));

      console.log('userdata . coin bal '+ userData.coin_balance);

      setCoinBalanceToDB(newCoin,id).then(new_balance=>{
          console.log('this is the new balance '+new_balance);
        connection.send(JSON.stringify({
          messageType:'coinsUpdate',
          coin: new_balance
      }));
      });
  });
  },1000,userId,wsConnection) 
}

function setCoinBalanceToDB(coin_balance,userId)
{
  const promise = new Promise((resolve,reject)=>{

  var updateQuery =`UPDATE users SET coin_balance=`+coin_balance+ ` WHERE id=`+userId+`;`;
  con.query(updateQuery, function (err, updateResults) {
    if(err)
       reject(false);
    else resolve( coin_balance);
  });
});
return promise;
}


function userTapped(userData) {

  console.log('userdata received in usertapped '+JSON.stringify( userData));
  let earn_per_tap = userData.earn_per_tap;
  let finalCoins = userData.coin_balance + earn_per_tap;
  const customPromise = new Promise((resolve, reject) => {
    var updateQuery =`UPDATE users SET coin_balance=`+finalCoins+ ` WHERE id=`+userData.id+`;`;
    con.query(updateQuery, function (err, updateResults) {
      if(err)
        reject(err);
      else
        resolve(true);
    });
  })

  return customPromise
}

function getUserData(userId)
{
  const promise = new Promise((resolve,reject)=>{

    var qry =`SELECT * FROM users WHERE id=`+userId;
    console.log('getUserData of this id '+ userId);
    con.query(qry, function (err, results) {
      console.log('err is '+ err);
      if (err)
         reject(err); 
      else
      {
        resolve(results[0]);
      }});

  });
    return promise;
}




//#endregion

//#region DB connection
var mysql = require('mysql');
var con = mysql.createConnection({
   host: "localhost",
   user: "root",
   password: "1234",
   database: "hamster_data"
});
con.connect(function (err) {
   if (err) throw err;
   console.log("DB Connected!");

});
//#endregion


//#region requests




app.get('/', (req, res) => {
  res.send('Hello World!');
});

app.post('/login',urlencodedParser, (req, res) => {
 getUserData(req.body.userId,res).then(data=>{
  res.end(JSON.stringify(data));

 });

});

app.post('/tapped',urlencodedParser, (req, res) => {
    getUserData(req.body.userId).then
    (data=>{
      userTapped(data).then(userTappedData=>
      {
        getUserData(req.body.userId).then(secondUserData=>{
            res.end(JSON.stringify(secondUserData));
        });
      });
    });
});


app.post('/buyMiningCard',urlencodedParser, (req, res) => {
  buyMiningCard(req.body.cardId,req.body.userId);
});



app.get('/createNewUser', (req, res) => {
console.log('inside create function');
  var qry =`INSERT INTO users (name) VALUES('navid');`;
  var getLastRowQuery =`      SELECT    *
FROM      users
ORDER BY  id DESC
LIMIT     1;`;
  con.query(qry, function (err, results) {
    console.log('err is '+ err);
    if (err)
       throw err;
    else
    {
        con.query(getLastRowQuery,function(error, result){
            if(error)
              throw error;
            else res.send(JSON.stringify({user:result}));
            console.log("result of get last row query "+JSON.stringify({user:result}));

        });

    } 
    console.log(JSON.stringify(results));
 });
});


app.get('BuyCard',(req,res)=>{
    console.log('req body is '+ req.body);
});
app.get('/GetMiningCards', (req, res) => {
  let selectQuery = 'SELECT * FROM cards;'
  con.query(selectQuery, function (err, results) {
    console.log('err is '+ err);
    if (err)
       throw err;
    else
    {
      let deserializedResult = {cards: results};
       res.send(deserializedResult);
    }
  }
)});

app.listen(port, () => {
  console.log(`Example app listening at http://localhost:${port}`);
});

app.use('/images', express.static('images'));
//#endregion