

$.formattedDate = function (dateToFormat) {

    /* EJEMPLO DE USO
    var randomDate = new Date("2015-09-30"); FORMATO 1
    var epochTime = new Date(1263183045000); FORMATO 2
    var jsonResult = "\/Date(1263183045000)\/"; FORMATO 3

    //outputs "30/09/2015"
    alert($.formattedDate(randomDate));  FORMATO 1
    //both below output "11/01/2010"
    alert($.formattedDate(epochTime)); FORMATO 2
    alert($.formattedDate(new Date(parseInt(jsonResult.substr(6)))));  FORMATO 3
    */

    var dateObject = new Date(dateToFormat);
    var day = dateObject.getDate();
    var month = dateObject.getMonth() + 1;
    var year = dateObject.getFullYear();
    day = day < 10 ? "0" + day : day;
    month = month < 10 ? "0" + month : month;
    var formattedDate = day + "/" + month + "/" + year;
    return formattedDate;
};

$.formattedDateyyyyMMdd = function (dateToFormat) {

    /* EJEMPLO DE USO
    var randomDate = new Date("2015-09-30"); FORMATO 1
    var epochTime = new Date(1263183045000); FORMATO 2
    var jsonResult = "\/Date(1263183045000)\/"; FORMATO 3

    //outputs "30/09/2015"
    alert($.formattedDate(randomDate));  FORMATO 1
    //both below output "11/01/2010"
    alert($.formattedDate(epochTime)); FORMATO 2
    alert($.formattedDate(new Date(parseInt(jsonResult.substr(6)))));  FORMATO 3
    */

    var dateObject = new Date(dateToFormat);
    var day = dateObject.getDate();
    var month = dateObject.getMonth() + 1;
    var year = dateObject.getFullYear();
    day = day < 10 ? "0" + day : day;
    month = month < 10 ? "0" + month : month;
    var formattedDate = year + "-" + month + "-" + day;
    if (year < 2000) {
        formattedDate = ""
    }
    return formattedDate;
};


$.subtractDates = function (date1, date2) {
    // Convertir las fechas del formato /Date(123456789)/ a objetos Date
    function convertDate(dato) {
        if (!dato || typeof dato !== 'string') {
            console.error("El valor de fecha no es válido:", dato, typeof (dato));
            return ' ';
        }

        const match = dato.match(/\/Date\((-?\d+)\)\//);
        if (match) {
            const timestamp = parseInt(match[1], 10);
            return new Date(timestamp);
        }

        console.error("El valor de fecha no es válido:", dato, typeof (dato));
        return ' ';
    }

    // Convertir las fechas
    const fecha1 = convertDate(date1);
    const fecha2 = date2 ? convertDate(date2) : new Date(); // Si date2 es nulo, usar la fecha actual

    if (!fecha1) {
        console.error("Fecha1 no es válida.");
        return 0; // Si fecha1 no es válida, devolver 0
    }

    // Calcular la diferencia en milisegundos
    const diferenciaTiempo = fecha2 - fecha1;

    if (diferenciaTiempo >= 0) {
        return Math.floor(diferenciaTiempo / (1000 * 60 * 60 * 24)); // Convertir a días
    } else {
        return 0; // Si la diferencia es negativa, asignar 0
    }
};
