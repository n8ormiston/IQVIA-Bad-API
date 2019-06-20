$('#get-tweets').on('click', function () {
    
    let startDate = $('#start-date').val();
    let endDate = $('#end-date').val();

    if (!Date.parse(startDate) ||
        !Date.parse(endDate)) {
        bootbox.alert("Please enter valid dates for both start and end date.");
    }
    else {
        
        let url = $(this).data('action-url');
        $('#tweet-results').addClass('hidden');
        $('#loading-gif').removeClass('hidden');

        $.ajax({
            'url': url,
            'type': 'GET',
            'data': {
                'startDate': startDate,
                'endDate': endDate
            },
            'success': function (data) {
                $('#tweet-results').html(data);
                $('#tweet-results').removeClass('hidden');
                $('#loading-gif').addClass('hidden');
                
            },
            'error': function (request, error) {
                alert("Request: " + JSON.stringify(request));
            }
        });
    }
    
});