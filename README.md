# Refactoring workshop

Awesome Corp is planning on moving from Rise CRM to Tradeclout CRM. The code provided here covers the login journey and the entry point can be found at

AwesomeCorp/Services/Impl/MembershipService/AuthenticateUser

The devs at Tradeclout have promised that CRM objects will not be changed, all fields will keep the same name and type, with one exception. CRM profile IDs in Rise are represented by a GUID but in Tradeclout they are an 18 character alphanumeric string.

How confident would you be in making the necessary code changes?

What could you do to improve your confidence?
