new DataTable('#tablo1');
new DataTable('#tablo3'); new DataTable('#tablox');



    function combovalue(){
    var comboBox = document.getElementById("ComboBox");
    var selectedOptionValue = comboBox.value;
    var inp1 = document.getElementById("comboinput");
    inp1.value=selectedOptionValue;
}

function randevalue() {
    var comboBox = document.getElementById("myDropdown");
    var selectedOptionValue = comboBox.value;
    var inp1 = document.getElementById("hastaid");
    inp1.value = selectedOptionValue;
}
$(document).ready(function () {
    $('#tablo3').dataTable({
        { "dom", "lfrtp" }
    });
});