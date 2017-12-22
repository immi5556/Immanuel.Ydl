var request = new XMLHttpRequest();
    request.onreadystatechange = function () {
if (this.readyState == 4 && this.status == 200) {
    var type = request.getResponseHeader("Content-Type");
    var disposition = request.getResponseHeader("Content-Disposition");
    var fname = (getFileName(disposition) || 'Temp') + '.' + getMimes(type);
    saveData(this.response, fname, type, )
}
};
request.open('GET', 'https://jsonmock.hackerrank.com/api/movies/search/?Title=spider');

