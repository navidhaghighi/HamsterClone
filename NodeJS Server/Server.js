const express = require('express');
const app = express();
const port = 3000;

app.get('/', (req, res) => {
  res.send('Hello World!');
});

app.get('/GetMiningCards', (req, res) => {


 /* var mysql = require('mysql');
var con = mysql.createConnection({
   host: "localhost",
   user: "root",
   password: "mypassword",
   database: "mydb"
});
  var qry =`SELECT name, salary, salary*0.05 as tax FROM employee;`;
  con.connect(function (err) {
     if (err) throw err;
     console.log("Connected!");
     con.query(qry, function (err, results) {
        if (err) throw err;
        console.log(JSON.stringify(results));
     });
     con.end();
  });*/
  var cards = [
    [ 'name','first',
      'initialProfit','30'
    ],
    [ 'name', 'dasd',
    'initialProfit','30'
    ],
    [ 'name','vxcvx',
      'initialProfit','30'
    ]
  ];
    res.setHeader('Content-Type', 'application/json');
    res.end(JSON.stringify(  
      {  
        cards: [
          {
            name: 'BestProfit',
            initialProfit: 300,
            reward: {
              coins: 300,
            }
          }
        ,
            {
            name: 'LeastProfit',
            initialProfit: 150,
            reward: {
              coins:100,
            }
          }
        
             ]
        }));
  });

app.listen(port, () => {
  console.log(`Example app listening at http://localhost:${port}`);
});