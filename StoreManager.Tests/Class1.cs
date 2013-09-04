using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace StoreManager.Tests
{
    public class Class1
    {

//        void Foo()
//        {
//            var request = new Mock<HttpRequestBase>();
//            request.SetupGet(x => x.IsAuthenticated).Returns(true); // or false

//            var context = new Mock<HttpContextBase>();
//            context.SetupGet(x => x.Request).Returns(request.Object);

//            var controller = new YourController();
//            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

//            // test

//            ViewResult viewResult = (ViewResult)controller.SomeAction();

//            Assert.True(viewResult.ViewName == "ViewForAuthenticatedRequest");


//        }

//        void Bar()
//        {
//            Testing Action Methods in Authenticated Controllers

//Mock HttpRequestBase,
//  Set IsAuthenticated to return True
//Mock HttpContextBase
//  Set Http to return mocked HttpRequestBase
  
//Declare controller. Set ControllerContext to new ControllerContext with mocked HttpContextBase, new RouteData() and your controller



//// mock context variables
//var username = "user";
//var httpContext = MockRepository.GenerateMock<HttpContextBase>();
//var request = MockRepository.GenerateMock<HttpRequestBase>();
//var identity = MockRepository.GenerateMock<IIdentity>();
//var principal = MockRepository.GenerateMock<IPrincipal>();

//httpContext.Expect( c = c.Request ).Return( request ).Repeat.AtLeastOnce();
//request.Expect( r = > r.IsAuthenticated ).Return( true ).Repeat.AtLeastOnce();
//httpContext.Expect( c => c.User ).Return( principal ).Repeat.AtLeastOnce();
//principal.Expect( p => p.Identity ).Return( identity ).Repeat.AtLeastOnce();
//identity.Expect( i => i.Name ).Return( username ).Repeat.AtLeastOnce();

//var controller = new MyController();
//// inject context
//controller.ControllerContext = new ControllerContext( httpContext,
//                                                      new RouteData(),
//                                                      controller );

//var result = controller.MyAction() as ViewResult;

//Assert.IsNotNull( result );

//// verify that expectations were met
//identity.VerifyAllExpectations();
//principal.VerifyAllExpectations();
//request.VerifyAllExpectations();
//httpContext.VerifyAllExpectations();




















//        }
    }
}
