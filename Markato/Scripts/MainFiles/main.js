$.noConflict();

jQuery(document).ready(function ($) {

    "use strict";

    [].slice.call(document.querySelectorAll('select.cs-select')).forEach(function (el) {
        new SelectFx(el);
    });

    jQuery('.selectpicker').selectpicker;


    $('#menuToggle').on('click', function (event) {
        $('body').toggleClass('open');
    });

    $('.search-trigger').on('click', function (event) {
        event.preventDefault();
        event.stopPropagation();
        $('.search-trigger').parent('.header-left').addClass('open');
    });

    $('.search-close').on('click', function (event) {
        event.preventDefault();
        event.stopPropagation();
        $('.search-trigger').parent('.header-left').removeClass('open');
    });
    $('.EditProject').on("click", function () {
        $('.projectDesc').toggle();
        $('.DescArea').toggle();
        $('.EditProject').text($(this).text() == 'Edit' ? 'Done' : 'Edit');
    });
    $(".fButton").on("click", function () {
        $(".Pform").css();
    });
    // handle links with @href started with '#' only
    $(document).on('click', 'a[href^="#"]', function (e) {
        // target element id
        var id = $(this).attr('href');

        // target element
        var $id = $(id);
        if ($id.length === 0) {
            return;
        }

        // prevent standard hash navigation (avoid blinking in IE)
        e.preventDefault();

        // top position relative to the document
        var pos = $id.offset().top;

        // animated top scrolling
        $('body, html').animate({ scrollTop: pos });
    });
    var colorNames = Object.keys(window.chartColors);

   $(".Signin").on('click', function () {
        var data = $("#signin").serialize();
        var url = "/Markato/Signin";
        $.post(url, data, function () { });
    });
    $(".SUSignin").on('click', function () {
        var form = $("#signin");
        var data = $(form).serialize();
        var url;
       /* if ($(".mt").is(":checked")) {
            $(form).attr("action", "/Markato/Signin");
            url = "/Markato/Signin";
        } else {
            $(form).attr("action", "/Markato/CustomerSignin");
            url = "/Markato/CustomerSignin";
        }*/
            
            $.post(url, data, function () { });
    });

    $("body").on("click", '.EditP', function () {
        var pid = $(this).prop("id");
        $("#" + pid).find('.Product-desc').toggle();

        $("#" + pid).find('.editDesc').toggle();
        $("#" + pid).find(".editDesc").val($("#" + pid).find('.Product-desc').text());
        $("#" + pid).find('.Product-name').toggle();
        $("#" + pid).find('.Editpname').toggle();
        $("#" + pid).find(".Editpname").val($("#" + pid).find('.Product-name').text());
        $("#" + pid).find('.EditP').addClass("SaveEdit");
        $("#" + pid).find('.EditP').removeClass("EditP");
    });
    $("body").on("click", '.SaveEdit', function () {
        var pid = $(this).prop("id");
        var desc = $("#" + pid).find('.editDesc').val();
        var name = $("#" + pid).find('.Editpname').val();
        $("#" + pid).find('.Product-desc').toggle();
        $("#" + pid).find('.Product-desc').text(desc);
        $("#" + pid).find('.editDesc').toggle();
        $("#" + pid).find('.Editpname').toggle();
        $("#" + pid).find('.Product-name').text(name);
        $("#" + pid).find('.Product-name').toggle();
        //alert(desc+name)
        $("#" + pid).find('.SaveEdit').addClass("EditP");
        $("#" + pid).find('.SaveEdit').removeClass("SaveEdit");
        var srda = { pid: pid, Desc: desc, Name: name };
        $.ajax({
            type: "POST",
            url: "/Projects/EditProject",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(srda),
            dataType: "html"
        });
    });
    $(".MD-card").on("click", function () {/*        var name = $(this).find("h5").text();
        var bio = $(this).find(".bio").text();
        var date = $(this).find(".dates").text();
        var can = $(this).find("canvas").clone();
        var src = $(this).find(".card-img-top").attr("src");
        var cc = "<div class='about-card-canv' style='margin: 0; padding: 0'> </div >";
        $(".cardabout").find(".card-body").append(cc);
        can.appendTo(".about-card-canv");
        $(".about-card-canv").find("canvas").css("display", "inline");
        $(".MD-bio").text(bio);
        $(".MD-date").text(date);
        $(".MD-name").text(name);
        $(".aboud-card-img").attr("src", src);
        $(".about-container").fadeIn(500);*/
    });
    $(".fa-times-circle").on("click", function () {
        $(".about-card-canv").remove();
        $(".cardabout").append(".about-card-canv");
        $(".about-container").fadeOut(500);
    });
    $(".Register").on("click", function () {
        var data = $("#RegisterForm").serialize();
        var url = $("#RegisterForm").prop("action");
        $.post(url, data, function () { });
    });
    $("body").on("click", '.MDSendRequest', function () {
        $(this).attr('disabled', 'true');
        $(this).text("Request Sent");
        var mdid = $(this).prop("id");
        var pid = $(this).closest(".product-card").prop("id"); 
        var srda = { pid: pid, mdid: mdid };
        $.ajax({
            type: "POST",
            url: "/Requests/MDSendRequest",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(srda),
            dataType: "html"
        });
    });
    $("body").on("click", '.SendRequest', function () {
        $(this).attr('disabled', 'true');
        $(this).text("Request Sent");
        var empmail = $(this).closest(".form-group").find("input").val();
        //var pid = $(this).prop("id");

        var pid = $(this).closest(".product-card").prop("id");
        var srda = { pid: pid, EmpMail: empmail };
       
        $.ajax({
            type: "POST",
            url: "/Requests/send",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(srda),
            dataType: "html"
        });
    });
    $("body").on("click", '.CustomerAccept', function () {
        $(this).attr('disabled', 'true');
        $(this).text("Request Sent");
        var mdid = $(this).prop("id");
        var pid = $(this).closest(".product-card").prop("id");
        var srda = { pid: pid, mdid: mdid };
        $.ajax({
            type: "POST",
            url: "/Customers/SendRequest",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(srda),
            dataType: "html"
        });
    });
    $("body").on("click", '.SetSchedual', function () {
        var projectid = $(this).prop("id");
        var pe = $(this).closest(".SchedualForm").find(".EndDate").val();
        $(this).attr('disabled', 'true');
        $(this).text("Schedual Set");
        var da = { projectid: projectid, pe: pe };
        $.ajax({
            type: "POST",
            url: "/Projects/SetSchedual",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(da),
            dataType: "html",
            error: function (jqXHR, status, err) {
                alert(jqXHR.responseText);
            }
        });
    });
    $(".profile").on("click", function () {
        var data = new String();
        data = $(this).closest("form").serialize();
        
        var url;
        $.post(url,data, function () { });
    });
    $("body").on("click", '.Pbutton', function () {
        var proid = $(this).prop("id");
        var price = $(this).closest(".form-group").find("input").val();
        $(this).closest(".form-group").find("input").attr('disabled', 'true');
        $(this).text("Price Set");
        alert(price);
        var data = { proid: proid, pp: price };
        $.ajax({
            type: "POST",
            url: "/Projects/SetPrice",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(data),
            dataType: "html",
        });

    });
    $("body").on("click", ".NewMember", function () {
        var mainid = $(this).prop("id");
        var id = '#' + $(this).prop("id");
        var c = $(".AddMemberDiv" + id);
        $(".MainF:first").clone().css("display", "inherit").removeClass('MainF').appendTo($(id).find(".AddMemberDiv"));
    });
    $("body").on("click", ".MDAcceptProject", function () {
        var Eid = $(this).prop("id");
        var Pid = $(this).closest(".product-card").prop("id");
        var id = "#" + Pid;
        AppendToPostTemplate(id, Pid);
        $(id).remove();
        var data = { Eid: Eid, Pid: Pid };

        $.ajax({
            type: "POST",
            url: "/Projects/MDAccProject",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(data),
            dataType: "html",
        });
    });
    $("body").on("click", ".LAcceptProject", function () {
        var Eid = $(this).prop("id");
        var Pid = $(this).closest(".product-card").prop("id");
        var id = "#" + Pid;
        AppendToPostTemplate(id, Pid);
        $(id).remove();
        var data = { Eid: Eid, Pid: Pid };

        $.ajax({
            type: "POST",
            url: "/Teams/LeaderAccProject",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(data),
            dataType: "html",
        });
    });
    $("body").on("click", ".RejectProject", function () {
        var Eid = $(this).prop("id");
        var Pid = $(this).closest(".product-card").prop("id");
        var id = "#" + Pid;   
        $(id).remove();
        var data = { Eid: Eid, Pid: Pid };

        $.ajax({
            type: "POST",
            url: "/Requests/Del",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(data),
            dataType: "html",
        });
    });
    $("body").on("click", ".RemoveML", function () {
        var pid = $(this).prop("id");
        var teamML = $(this).closest(".TeamML");
        var id = "#" + $(this).prop("id");
        $(this).closest(".product-card").find(".dorpML").remove();
        teamML.append("<form method='Post'><div class='form-group row col-12'><div class='col-sm-7'><input type='text' class='form-control' id='' placeholder='Leader'></div><div class='col-sm-5'><button type='button' class='btn btn-outline-success SendRequest' id=''>Send Request</button></div></div></form>");
        var data = { ProjectId: pid };
        $.ajax({
            type: "POST",
            url: "/Projects/RemoveML",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(data),
            dataType: "html",
        });
    });
    $("body").on("click", ".RemoveMT", function () {
        var tid = $(this).prop("id");
        var teamid = $(this).closest(".TeamMembers").prop("id");
        var id = "#" + tid;
        $(this).closest(".TeamMembers").find(".MTdd" + id).remove();
        var data = { TeamID: teamid, id: tid };
        $.ajax({
            type: "POST",
            url: "/Projects/RemoveMT",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(data),
            dataType: "html",
        });
    });
    $("body").on("click", ".leaveProject", function () {

        var id = $(this).prop("id");
        $('.AssignedProjects').find(".product-card" + "#" + id).remove();
        var data = { pid: id };
        $.ajax({
            type: "POST",
            url: "/Projects/LeaveProject",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(data),
            dataType: "html",
        });
    });
    $("body").on("click", ".DeleteProject", function () {

        var id = $(this).prop("id");
        $("#"+id).closest(".product-card").remove();
        $('.AssignedProjects').find(".product-card" + "#" + id).remove();
        var data = { pid: id };
        $.ajax({
            type: "POST",
            url: "/Projects/DeleteProject",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(data),
            dataType: "html",
        });
        
    });
    $("body").on("click", ".LleaveProject", function () {
        var pid = $(this).prop("id");
        $("#" + pid).remove();
        var data = { pid: pid };
        $.ajax({
            type: "POST",
            url: "/Teams/LeaveProject",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(data),
            dataType: "html",
        });

    })
    $("body").on("click", ".CloseProject", function () {
        var oid = $(this).prop("id");
        var id = "#" + oid;
        AppendToHistoryTemplate(id, oid);
        $(id).remove();
        var data = { pid: oid };
        $.ajax({
            type: "POST",
            url: "/Projects/CloseProject",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(data),
            dataType: "html",
        });

    });  
    $("body").on("click", ".ShowFeedback", function () {
        $(".new").remove();
        var Tid = $(this).prop("id");
        var Hd = $(this).closest("form").find(".HiddenData");
        var Pid = $(this).closest(".product-card").prop("id");
        var copy = $(".FeedbackBlock").first().clone();
        $(copy).css("display", "block").find("button").attr("id", Tid);
        $(copy).find(".fb").val($(Hd).find(".hfb").text());
        $(copy).find(".r").val(parseInt($(Hd).find(".hr").text()));
        $(copy).addClass("new");

        $("#" + Pid).find(".MT").append($(copy));
    });
    $("body").on("click", ".SendFeedBack", function () {
        var Tid = $(this).prop("id");
        var Pid = $(this).closest(".product-card").prop("id");
        var fb = $(this).closest(".card").find(".fb").val();
        
        var r = $(this).closest(".card").find(".r").val();
        alert(fb);
        var data = { pid: Pid, eid: Tid, r: r, fb: fb };
        $(this).closest(".FeedbackBlock").remove();
        $.ajax({
            type: "POST",
            url: "/FeedBacks/Create",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(data),
            dataType: "html",
        });

    });
    $("body").on("click", ".CloseFb", function () {
        $(this).closest(".FeedbackBlock").remove();
    });
    $("body").on("click", ".MTleaveProject", function () {
        var tid = $(this).closest(".row").prop("id");
        var eid = $(this).prop("id");
        $(this).text("Request Sent").attr("disabled", "true");
        var data = { tid: tid, eid: eid };
        $.ajax({
            type: "POST",
            url: "/ProjectTeamHistories/Request",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(data),
            dataType: "html",
        });
    });
    $("body").on("click", ".MTAccProject", function () {
        var Eid = $(this).prop("id");
        var Pid = $(this).closest(".product-card").prop("id");
        var Tid = $(this).closest(".row").prop("id");
        alert(Eid + " " + Pid + " " + Tid);
        var id = "#" + Pid;
        AppendToPostTemplate(id, Pid);
        $(id).remove();
        var data = { Eid: Eid,Pid: Pid,Tid:Tid }; 
        $.ajax({
            type: "POST",
            url: "/ProjectTeamHistories/TAccProject",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(data),
            dataType: "html",
        });
    });
    $("body").on("click", ".RejLeaveReq", function () {
        var tid = $(this).prop("id");
        var teamid = $(this).closest(".TeamMembers").prop("id");
        alert("xsda");
        var data = { TeamID: teamid, id: tid };
        $.ajax({
            type: "POST",
            url: "/ProjectTeamHistories/RemoveReq",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(data),
            dataType: "html",
        });
    });
    $(".CreateProject").on("click", function () {
        var form = $(".CreatForm");
        var name = $(form).find("#ProductName").val();
        var Desc = $(form).find("#ProductDesc").val();
        var cid = $(form).prop("id");
        var data = { desc: Desc, name: name, cid: cid };

        $.ajax({
            type: "POST",
            url: "/Projects/CreateProject",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(data),
            dataType: "html",
        });

        window.location.reload();
    });
    function AppendToHistoryTemplate(PostID, id) {
        var img = $(PostID).find('.product-img').prop('src');
        var pname = $(PostID).find('.Product-name').text();
        var pdesc = $(PostID).find('.Product-desc').text();
        var Cname = $(PostID).find(".Custmer-name").text();
        var date = $(PostID).find(".Date").text();
        var sdate = $(PostID).find(".StartDate").val();
        var edate = $(PostID).find(".EndDate").val();
        var MLname = $(PostID).find(".MLname").text();
        var MLlink = $(PostID).find(".MLProfile").prop('href');
        var copy = $(".HistoryCardTemblate").clone().removeClass('HistoryCardTemblate');
        $(copy).attr('id', id);
        $(copy).find('.product-img').attr('src', img);
        $(copy).find('.Date').text(date);
        $(copy).find(".MLname").text(MLname);
        $(copy).find(".ML").find('a').attr('href', MLlink);
        $(copy).find('.Custmer-name').text(Cname);
        $(copy).find('.Product-name').text(pname);
        $(copy).find('.Product-desc').text(pdesc);
        $(copy).find('.StartDate').val(sdate);
        $(copy).find('.EndDate').val(edate);
        var mems = $(copy).find('.MTdd').clone();
        $(copy).find('.MTdd').remove();
        $(PostID).find('.MTdd').each(function () {
            var mem = mems.clone();
            mem.find('button').text($(this).find('.btn-info').text());
            mem.attr('href', $(this).find('a').attr('href'));
            mem.appendTo(copy.find('.TeamMembers'));
        });
        $(copy).css("display", "inline");
        $('.Histories').append($(copy));
    }
    function AppendToPostTemplate(PostID, id) {
        var img = $(PostID).find('.product-img').prop('src');
        var pname = $(PostID).find('.Product-name').text();
        var pdesc = $(PostID).find('.Product-desc').text();
        var Cname = $(PostID).find(".Custmer-name").text();
        var date = $(PostID).find(".Product-date").text();
        var copy = $(".ProductCardTemplate").clone().removeClass('ProductCardTemplate');
        $(copy).attr('id', id);
        $(copy).find('.product-img').attr('src', img);
        $(copy).find('.Date').text(date);
        $(copy).find('.CustmerName').text(Cname);
        copy.find('.Product-name').text(pname);
        $(copy).find('.Product-desc').text(pdesc);
        $(copy).css("display", "inline");
        $('.AssignedProjects').append($(copy));
    }
    
    
       
    
});