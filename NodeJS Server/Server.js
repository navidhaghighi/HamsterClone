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

function upgradeCard(userId,cardId)
{
  const promise = new Promise((resolve,reject)=>{
  var updateQuery =`UPDATE usercards SET current_level=current_level+1 WHERE user_id=`+userId + '&cardId='+cardId+';';
  con.query(updateQuery, function (err, updateResults) {
    if(err)
       reject(false);
    else resolve(true);
  });
});
return promise;
}


function buyMiningCard(cardId,userId)
{
  const promise = new Promise((resolve,reject)=>{
    var insertQuery =`INSERT INTO usercards(card_id,user_id,current_level) VALUES(`+ cardId+`,`+userId+`,1);`;
    con.query(insertQuery, function (err, updateResults) {
      if(err)
         reject(false);
      else resolve(true);
    });
  });
  return promise;

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
  const promise = new Promise((resolve,reject)=>{
console.log('in user tapped sending this '+userData.earn_per_tap );
  increaseCoins(userData,userData.earn_per_tap).then(data=>{
    if(data!=null)
      resolve(data);
    else reject();
  });
});
return promise;
}
///increase coins then return the user object as the result
function increaseCoins(user,increaseAmount)
{
  const promise = new Promise((resolve,reject)=>{
    getNextRankCoinsRequired(user).then(result=>{
      console.log('the user is '+ user);
      console.log('the increase '+ increaseAmount);

    let newAmount =  user.coin_balance += increaseAmount;

      var addCoinsQuery =`UPDATE users SET coin_balance=`+newAmount+`, coins_to_level_up=`+result+` WHERE id=`+user.id+`;`;
      con.query(addCoinsQuery, function (err, results) {
        if (err)//if there is no rank above this , then user has reached that last rank
           reject(err); 
        else
        {
          console.log('last log '+ results[0]);
          resolve(true);
        }});
    });

  });
    return promise;

}
///how many coins user needs to reach the next rank
//get next rank first, the next rank is whichever rank is after this one (you can find out which one is next 
//either by rankId)
function getNextRankCoinsRequired(user)
{
  console.log("getNextRankCoinsRequired user id "+ user)
  const promise = new Promise((resolve,reject)=>{
    //get user current rank
    console.log('user is '+ (user.current_rank+1));
    var qry =`SELECT * FROM  ranks WHERE id=`+(user.current_rank+1);
    console.log('after select '+ user.current_rank);
    con.query(qry, function (err, results) {
      let currentRank  = results[0];
      console.log("next rank is "+ results[0]);
      if (err)
        {//if there is no rank above this , then user has reached that last rank
          console.log('there was error '+ err);
         reject(err); 
        }
      else
      {
        console.log('before resolve '+results );
        resolve( results[0].coins_required - user.coin_balance );
      }});
  });
    return promise;

}




function getUserData(userId)
{
  const promise = new Promise((resolve,reject)=>{

    var qry =`SELECT * FROM users WHERE id=`+userId;
    con.query(qry, function (err, results) {
      if (err)
         reject(err); 
      else
      {
        resolve(results[0]);
      }});

  });
    return promise;
}

function getUserCards(userId)
{
  const promise = new Promise((resolve,reject)=>{

    var qry =`SELECT * FROM usercards WHERE user_id=`+userId;
    con.query(qry, function (err, results) {
      console.log('err is '+ err);
      if (err)
         reject(err); 
      else
      {
        resolve(results);
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
      userTapped(data,data.earn_per_tap).then(userTappedData=>
      {
        getUserData(req.body.userId).then(secondUserData=>{
            res.end(JSON.stringify(secondUserData));
        });
      });
    });
});

app.post('/getUserCards',urlencodedParser, (req, res) => {
  getUserCards(req.body.userId).then(data=>
  {
    console.log('responding to GetUserCards with ' +JSON.stringify(data));
    res.end (JSON.stringify( data));
  });
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

app.post('/upgradeCard',urlencodedParser,(req,res)=>{
  upgradeCard(req.body.userId,req.body.cardId).then(data=>{
    console.log('result is '+ data);

  });
  res.end();
});

app.post('/buyCard',urlencodedParser,(req,res)=>{
    buyMiningCard(req.body.cardId,req.body.userId);
    res.end();
});


app.get('/getMiningCards', (req, res) => {
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