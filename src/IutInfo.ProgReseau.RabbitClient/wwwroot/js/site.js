$(function() {
  var connection = new signalR.HubConnectionBuilder().withUrl("/rabbithub").build();

  var $content = $('#content');

  $('#form').on('submit',
    function(e) {
      e.preventDefault();

      connection.invoke("SendMessage", $("#text").val()).then(function() {
        $('#form')[0].reset();
      }).catch(function (err) {
        return console.error(err.toString());
      })
      /*
      $.ajax({
        url: '/post',
        method: 'post',
        data: $(this).serialize(),
        success: function() {
          $('#form')[0].reset();
        }
      });*/
    });

  connection.on("Rabbit", function (content) {
    var $li = $('<li>').text(content);
    $li.appendTo($content);
  });

  connection.start().then(function () {
  }).catch(function (err) {
    return console.error(err.toString());
  });
});