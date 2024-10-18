//*********************************//
//******** Réservé ACARS **********//
//*********************************//

function doGet(e) {
  if ((e !== undefined)&&(e.parameter.query!== undefined)) {

    //in case of badly formulated query, return the query expressed in json.
    var jsonResponse = e;

    if (e.parameter.query == "airports") {
      var lastUpdate = e.parameter.date;
      jsonResponse = createJSONAirports(lastUpdate);
    }

    if (e.parameter.query == "fleet") {
      var jsonFlotte = createJSONFlotte();
      var jsonMissions = createJSONMissions();
      jsonResponse = {
        flotte: jsonFlotte,
        missions: jsonMissions
      };     
    }
    
    if (e.parameter.query == "freight") {
      var airport = e.parameter.airport;
      jsonResponse = createJSONFreight(airport);
    }

    return ContentService.createTextOutput(JSON.stringify(jsonResponse))
    .setMimeType(ContentService.MimeType.JSON);
  }

  if ((e !== undefined)&&(e.parameter.page !== undefined)) {
    var htmlResponse=e;
    if (e.parameter.page == "fleet") {
      htmlResponse = getHTMLFleet();
    }
    if (e.parameter.page == "pilots") {
      htmlResponse = getHTMLPilots();
    }
    if (e.parameter.page == "flights") {
      htmlResponse = getHTMLFlights();
    }
    if (e.parameter.page == "mission") {
      var missionID = e.parameter.id;
      htmlResponse = getHTMLMission(missionID);
    }
    if (e.parameter.page == "flight") {
      var flightID = e.parameter.id;
      htmlResponse = getHTMLFlight(flightID);
    }
    return HtmlService.createHtmlOutput(htmlResponse);
  }
}

// *********************************** ///
// *** recup donnée depuis l'ACARS *** ///
// *********************************** ///

//function called to send data to the sheet
function doPost(e) {
 
  try {
    var jsonData = JSON.parse(e.postData.contents);
  } catch(error) {
    log(error.message);
  }

  if ((e !== undefined)&&(jsonData["query"]!== undefined)) {
    //in case of badly formulated query, return the query expressed in json.
    var jsonResponse = e;
    if (jsonData.query == "save") {
      //get the values of the flight
      var callsign = jsonData.cs;
      var immat = jsonData.plane;
      var sicao = jsonData.sicao;
      var sfuel = jsonData.sfuel;
      var stime = jsonData.stime;

      var eicao = jsonData.eicao;
      var efuel = jsonData.efuel;
      var etime = jsonData.etime;

      var cargo = jsonData.cargo;
      var mission = jsonData.mission;
      var note = jsonData.note;
      var comment = jsonData.comment;

      //create a new entry in the flight log book.
      var ongletFormulaire = SpreadsheetApp.getActiveSpreadsheet().getSheetByName("FROM_ACARS");
      var derniereLigne = ongletFormulaire.getLastRow();
      //on va remplir la ligne suivante
      derniereLigne++;
      var dateActuelle = new Date();
      var formattedDateTime = Utilities.formatDate(dateActuelle, Session.getScriptTimeZone(), "dd/MM/yyyy HH:mm:ss");

      ongletFormulaire.getRange("A" + derniereLigne).setValue(formattedDateTime);
      ongletFormulaire.getRange("B" + derniereLigne).setValue(callsign);
      ongletFormulaire.getRange("C" + derniereLigne).setValue(immat);
      ongletFormulaire.getRange("D" + derniereLigne).setValue(sicao);
      ongletFormulaire.getRange("E" + derniereLigne).setValue(sfuel);
      ongletFormulaire.getRange("F" + derniereLigne).setValue(stime);
      ongletFormulaire.getRange("G" + derniereLigne).setValue(eicao);
      ongletFormulaire.getRange("H" + derniereLigne).setValue(efuel);
      ongletFormulaire.getRange("I" + derniereLigne).setValue(etime);
      ongletFormulaire.getRange("J" + derniereLigne).setValue(cargo);
      ongletFormulaire.getRange("K" + derniereLigne).setValue(comment);
      ongletFormulaire.getRange("L" + derniereLigne).setValue(note);
      ongletFormulaire.getRange("M" + derniereLigne).setValue(mission);    
      
      result="OK";

    }
    //in case of badly formulated query, return the query expressed in json.
    var jsonResponse = e;
    if (jsonData.query == "updatePlaneStatus") {
      //get the values of the flight
      var callsign = jsonData.cs;
      var immat = jsonData.plane;
      var sicao = jsonData.sicao;
      var flying = jsonData.flying;
      var endICAO = jsonData.endIcao;

      //update FLOTTE sheet
      var ongletFlotte = SpreadsheetApp.getActiveSpreadsheet().getSheetByName("FLOTTE");
      //chercher la ligne du pilote
      var values = ongletFlotte.getDataRange().getValues();

      for (var i=0;i<values.length;i++) {
        if (values[i][3] == immat) {
          var ligne = i+1;
          ongletFlotte.getRange("J"+ligne).setValue(callsign);
          ongletFlotte.getRange("E"+ligne).setValue(sicao);
          ongletFlotte.getRange("M"+ligne).setValue(flying);
          if (flying === 1) {
            ongletFlotte.getRange("O"+ligne).setValue(sicao);
            ongletFlotte.getRange("P"+ligne).setValue(endICAO);
          } else {
            ongletFlotte.getRange("O"+ligne).clearContent();
            ongletFlotte.getRange("P"+ligne).clearContent();
          }
        }
      }           
      result="OK";
    }
  } 
  return ContentService.createTextOutput(result).setMimeType(ContentService.MimeType.TEXT);  
}

// *** envoie des données vers l'ACARS *** ///

function createJSONFlotte() {
  var sheet1 = SpreadsheetApp.getActiveSpreadsheet().getSheetByName("FLOTTE");
  var flotte = sheet1.getDataRange().getValues();
  return convertToJson(flotte);
}

function createJSONAirports(lastUpdate) {
  var airports;
  var sheet2 = SpreadsheetApp.getActiveSpreadsheet().getSheetByName("AEROPORTS");
  var serverLastUpdate = sheet2.getRange("O1").getValue();
  // Convertir la date en format JavaScript
  var dateObject = new Date(serverLastUpdate);
 
  // Convertir la date en epoch (millisecondes depuis le 1er janvier 1970)
  var epochMilliseconds = dateObject.getTime();
  if (lastUpdate < epochMilliseconds) {
    airports = sheet2.getDataRange().getValues();
    return convertToJson(airports);
  }
  return null 
}

function createJSONFreight(airport) {
  var found=false;
  Logger.log("looking for freight for airport " +airport );
  var sheet2 = SpreadsheetApp.getActiveSpreadsheet().getSheetByName("AEROPORTS");
  // Rechercher la cellule contenant la valeur passée en parametre
  var range = sheet2.getDataRange();
  var values = range.getValues();
  var valueFreight;

  for (var i = 1; i < values.length; i++) {
      if (values[i][0] === airport) {
        // Récupérer la valeur de la cellule à droite
          valueFreight={
            fret:values[i][12]
          };
          Logger.log("value is at line " +i +" value is" + valueFreight[0] );
          found=true;
          break;
      }
    if (found) {
      break;
    }
  }  
  return valueFreight;
}

function createJSONMissions() {
  var sheet3 = SpreadsheetApp.getActiveSpreadsheet().getSheetByName("MISSIONS");
  var missions = sheet3.getDataRange().getValues();
  return convertToJson(missions);
}

function convertToJson(data) {
  var headers = data[0];
  var jsonData = [];

  for (var i = 1; i < data.length; i++) {
    var row = data[i];
    var obj = {};
    for (var j = 0; j < headers.length; j++) {
      obj[headers[j]] = row[j];
    }
    jsonData.push(obj);
  }

  return jsonData;
}

// ************************************///
// *** construction des pages web  *** ///
// ************************************///

function getHTMLFleet() {
  var result="";
  var sheet3 = SpreadsheetApp.getActiveSpreadsheet().getSheetByName("FLOTTE");
  var fleet = sheet3.getDataRange().getValues();
  result+="<div>";
  result+="<table>";
  //write the table header
  var header=fleet[0];
    result+="<tr>";
  for (var c=0; c<header.length;c++) {
    result+="<th>"+header[c]+"</th>";
  }
  result+="</tr>";
  //pour chaque ligne
  for (var l=1; l<fleet.length;l++) {
    result+="<tr>";
    for (var c=0; c<header.length;c++) {
      result+="<td>"+fleet[l][c]+"</td>";
    }
    result+="</tr>";
  }
  result+="</table>";
  result+="</div>";
  console.log(result);
  return result;
}

function getHTMLPilots() {
  var result="Not implemented yet";
  console.log(result);
  return result;
}

function getHTMLFlights() {
  var result="Not implemented yet";
  console.log(result);
  return result;
}

function getHTMLMission(missionID) {
  var result="Not implemented yet";
  console.log(result);
  return result;
}
function getHTMLFlight(flightID) {
  var result="Not implemented yet";
  console.log(result);
  return result;
}

