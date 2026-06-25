<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerWeb.aspx.cs" Inherits="electricity.CustomerWeb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html lang="en">
<head>
<title>Customer Page</title>
<!-- custom-theme -->
<meta name="viewport" content="width=device-width, initial-scale=1">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta name="keywords" content="Project Responsive web template, Bootstrap Web Templates, Flat Web Templates, Android Compatible web template, 
Smartphone Compatible web template, free webdesigns for Nokia, Samsung, LG, SonyEricsson, Motorola web design" />
<script type="application/x-javascript"> addEventListener("load", function() { setTimeout(hideURLbar, 0); }, false);
		function hideURLbar(){ window.scrollTo(0,1); } </script>
<!-- //custom-theme -->
<!-- Google fonts -->
<link href="//fonts.googleapis.com/css?family=Open+Sans:300,300i,400,400i,600,600i,700,700i,800,800i&amp;subset=cyrillic,cyrillic-ext,greek,greek-ext,latin-ext,vietnamese" rel="stylesheet">
<link href="//fonts.googleapis.com/css?family=Raleway:100,100i,200,200i,300,300i,400,400i,500,500i,600,600i,700,700i,800,800i,900,900i&amp;subset=latin-ext" rel="stylesheet">
<!-- Google fonts -->

<!-- css files -->
<link href="web/css/style.css" type="text/css" rel="stylesheet" media="all">   
<!-- //css files -->

<link rel="stylesheet" href="web/css/font-awesome.css"> <!-- Font-Awesome-Icons-CSS -->

<link href="web/css/popup-box.css" rel="stylesheet" type="text/css" media="all" /> <!-- popup box css -->

</head>
<!-- body starts -->
<body>
<form id="form1" runat="server">
    
<!-- section -->
<section class="register">
	<div class="header">
		<div class="logo">
			<a href="#">Electricity Consumer</a>
		</div>
	<div class="social">
			<li><a href="index.aspx"><b>Log out ?<span class="fa fa-long-arrow-left"></span></b></a></li>
			
	</div>
		<div class="clear"></div>
	</div>

	<div class="register-full">
		<div class="register-left">
			<div class="register">
				<p></p>
				<h1>Hi!<asp:Label ID="lblname" runat="server" Text=""></asp:Label> </h1>
				<h2></h2>
				<p>Here you have privillages to Update and Manage your profile.</p>
				<div class="content3">
					<ul>
					<!--	<li><a class="" href="#"> New Project</a></li>
						<li><a class="read" href="#"> Get Started</a></li> -->
					</ul>
				</div>
			</div>
		</div>
		<div class="register-right">
     
			<div class="register-in">
				<a href="Customer.aspx" >Update Profile »</a>
			</div>
			<div class="register-in middle">
				<a href="Bills.aspx">Manage Bills »</a>
			</div>
			<div class="register-in">
				<a  href="UpdatePassword.aspx">Update Password »</a>
			</div>
           <div class="loading" align="center">
    Loading. Please wait.<br />
    <br />
   
</div>
			<div class="clear"> </div>
		</div>
	<div class="clear"> </div>
	</div>
	<!-- copyright -->
	<p class="agile-copyright">© 2018 Smart Energy Meter. All Rights Reserved | Smart Energy Meter</p>
	<!-- //copyright -->
</section>
<!-- //section -->



<!-- login form popup-->
<div class="pop-up"> 
	<div id="small-dialog" class="mfp-hide book-form">

		<div class="login-form login-form-left"> 
			<div class="agile-row">
				<h3>Login to your site</h3>
				<span class="fa fa-lock"></span>
				<div class="clear"></div>
				<div class="login-agileits-top"> 	
					<form action="#" method="post"> 
						<input type="text" class="name" name="user name" Placeholder="Username" required=""/>
						<input type="password" class="password" name="Password" Placeholder="Password" required=""/>
						<input type="submit" value="Login"> 
					</form> 	
				</div> 
				<div class="login-agileits-bottom"> 
					<h6><a href="#">Forgot password?</a></h6>
				</div> 
			</div>  
		</div> 
		
	</div>  
</div>
<!-- //login form popup-->

<!-- subscribe form popup-->
<div class="pop-up"> 
	<div id="small-dialog1" class="mfp-hide book-form">

		<div class="login-form login-form-left"> 
			<div class="agile-row">
				<h3>Be the first to hear</h3>
				<p>Sign up to get notified about latest offers, news and events. Stay tuned!</p>
				<div class="clear"></div>
				<div class="login-agileits-top"> 	
					<form action="#" method="post"> 
						<input type="text" class="name" name="name" Placeholder="Full Name" required=""/>
						<input type="email" class="email" name="email" Placeholder="Email" required=""/>
						<input type="submit" value="Get Notified"> 
					</form> 	
				</div>
			</div>  
		</div> 
		
	</div>  
</div>
<!-- //subscribe form popup-->

<!-- register form popup-->
<div class="pop-up"> 
	<div id="small-dialog2" class="mfp-hide book-form">
	
		<div class="login-form login-form-left"> 
			<div class="agile-row">
				<h3>Register form</h3>
				<div class="login-agileits-top"> 	
					<form action="#" method="post"> 
						<input type="text" class="name" name="name" Placeholder="Full Name" required=""/>
						<input type="email" class="email" name="email" Placeholder="Email" required=""/>
						<input type="text" class="phone" name="phone" Placeholder="Phone Number" required=""/>
						<input type="password" class="password" name="password" Placeholder="Password" required=""/>
						<div class="login-check">
							<label class="checkbox"><input type="checkbox" name="checkbox" checked=""><i> </i> I Accept to the <a href="#">Terms &amp; Conditions</a></label>
						</div>
						<input type="submit" value="Register"> 
					</form> 	
				</div>
			</div>
		</div>
		
	</div>
</div>
<!-- //register form popup-->

<!-- js -->

<script type="text/javascript" src="web/js/jquery-2.1.4.min.js"></script>
<!-- //js -->

<!--popup-js-->
<script src="web/js/jquery.magnific-popup.js" type="text/javascript"></script>
 <script>
     $(document).ready(function () {
         $('.popup-with-zoom-anim').magnificPopup({
             type: 'inline',
             fixedContentPos: false,
             fixedBgPos: true,
             overflowY: 'auto',
             closeBtnInside: true,
             preloader: false,
             midClick: true,
             removalDelay: 300,
             mainClass: 'my-mfp-zoom-in'
         });

     });
</script>
<!--//popup-js-->
<style type="text/css">
    .modal
    {
        position: fixed;
        top: 0;
        left: 0;
        background-color: black;
        z-index: 99;
        opacity: 0.8;
        filter: alpha(opacity=80);
        -moz-opacity: 0.8;
        min-height: 100%;
        width: 100%;
    }
    .loading
    {
        font-family: Arial;
        font-size: 10pt;
        border: 5px solid #67CFF5;
        width: 200px;
        height: 100px;
        display: none;
        position: fixed;
        background-color: White;
        z-index: 999;
    }
</style>
<script src="//code.jquery.com/jquery-2.1.1.min.js"></script>
<script type='text/javascript'>
    $(function () {
        // When a Button is clicked on your page, disable everything and display your loading element
        $(':a,:submit').click(function () {
            // Disable everything
            $('*').prop('disabled', true);
            // Display your loading image (centered on your screen)
            $('body').append("<img style='top: 45%; position: absolute; height: 100px; width: 100px;background: black;left: 45%;' src='http://www.klk.com.my/wp-content/themes/klk/images/loading-ajax.gif' />");
        });
    });
</script>

    
    
    </form>
</body>	
<!-- //body ends -->
</html>