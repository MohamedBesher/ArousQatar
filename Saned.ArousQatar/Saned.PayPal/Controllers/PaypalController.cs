using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.Owin.Security.Facebook;
using PayPal;
using PayPal.Api;
using Saned.ArousQatar.Data.Core;
using Saned.ArousQatar.Data.Core.Dtos;
using Saned.PayPal.Models;
using Saned.PayPal.Utilities;
using Saned.ArousQatar.Data.Core.Models;
using Saned.ArousQatar.Data.Persistence;

namespace Saned.PayPal.Controllers
{
    public class PaypalController : Controller
    {
        protected readonly IUnitOfWork _unitOfWork;

        public PaypalController()
        {
            _unitOfWork = new UnitOfWork();

        }

        //
        // GET: /Paypal/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetpayMentDetails()
        {

            // ### Api Context
            // Pass in a `APIContext` object to authenticate 
            // the call and to send a unique request id 
            // (that ensures idempotency). The SDK generates
            // a request id if you do not pass one explicitly. 
            // See [Configuration.cs](/Source/Configuration.html) to know more about APIContext.
            var apiContext = Configuration.GetAPIContext();

            // Specify a Payment ID to retrieve.  For demonstration purposes, we'll be using a previously-executed payment that used a PayPal account.
            var paymentId = "PAY-70T17199KT011811DLDXBW4A";

            // ^ Ignore workflow code segment
            // Retrieve the details of the payment.
            var payment = Payment.Get(apiContext, paymentId);

            string json = new JavaScriptSerializer().Serialize(payment);
            ViewBag.json = Common.FormatJsonString(json);

            // ^ Ignore workflow code segment

            return View();
        }


        //public ActionResult PaymentWithCreditCard()
        //{
            
        //}

        [HttpPost]
        public  ActionResult PaymentWithCreditCard()
        {
            // ### Api Context
            // Pass in a `APIContext` object to authenticate 
            // the call and to send a unique request id 
            // (that ensures idempotency). The SDK generates
            // a request id if you do not pass one explicitly. 
            // See [Configuration.cs](/Source/Configuration.html) to know more about APIContext.
            var apiContext = Configuration.GetAPIContext();

            // A transaction defines the contract of a payment - what is the payment for and who is fulfilling it. 
            var transaction = new Transaction()
            {
                amount = new Amount()
                {
                    currency = "USD",
                    total = "7",
                    details = new Details()
                    {
                        shipping = "1",
                        subtotal = "5",
                        tax = "1"
                    }
                },
                description = "This is the payment transaction description.",
                item_list = new ItemList()
                {
                    items = new List<Item>()
                    {
                        new Item()
                        {
                            name = "Item Name",
                            currency = "USD",
                            price = "1",
                            quantity = "5",
                            sku = "sku"
                        }
                    },
                    shipping_address = new ShippingAddress
                    {
                        //city = "Johnstown",
                        //country_code = "US",
                        //line1 = "52 N Main ST",
                        //postal_code = "43210",
                        //state = "OH",
                        //recipient_name = "Joe Buyer"


                        city = "Johnstown",
                        country_code = "SA",
                        line1 = "52 N Main ST",
                        postal_code = "43210",
                        state = "SA",
                        recipient_name = "Joe Buyer"
                    }
                },
                invoice_number = new Random().Next(999999).ToString()
            };

            // A resource representing a Payer that funds a payment.
            var payer = new Payer()
            {
                payment_method = "credit_card",
                funding_instruments = new List<FundingInstrument>()
                {
                    new FundingInstrument()
                    {
                        credit_card = new CreditCard()
                        {
                            billing_address = new Address()
                            {
                                city = "Johnstown",
                                country_code = "US",
                                line1 = "52 N Main ST",
                                postal_code = "43210",
                                state = "US"
                            },
                            cvv2 = "874",
                            expire_month = 07,
                            expire_year = 2020,
                            first_name = "Mohamed",
                            last_name = "Besher",
                            number = "6011041692799131",
                            type = "discover"
                        }
                    }
                },
                payer_info = new PayerInfo
                {
                   email = "dev.personal2@discoverAmerican.com"
                }
            };

            // A Payment resource; create one using the above types and intent as `sale` or `authorize`
            var payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = new List<Transaction>() { transaction }
            };



            try
            {
                    var createdPayment = payment.Create(apiContext);

                    if (createdPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                    return View();
            }
            catch (PayPalException ex)
            {
                Logger.Log("Error: " + ex.Message);
                return View("FailureView");
            }
            // Create a payment using a valid APIContext
           
            // For more information, please visit [PayPal Developer REST API Reference](https://developer.paypal.com/docs/api/).
        }



        //        public ActionResult PaymentWithCreditCard()
        //        {
        //            //create and item for which you are taking payment
        //            //if you need to add more items in the list
        //            //Then you will need to create multiple item objects or use some loop to instantiate object
        //            Item item = new Item();
        //            item.name = "Demo Item";
        //            item.currency = "USD";
        //            item.price = "5";
        //            item.quantity = "1";
        //            item.sku = "sku";

        //            //Now make a List of Item and add the above item to it
        //            //you can create as many items as you want and add to this list
        //            List<Item> itms = new List<Item>();
        //            itms.Add(item);
        //            ItemList itemList = new ItemList();
        //            itemList.items = itms;

        //            //Address for the payment
        //            Address billingAddress = new Address();
        //            billingAddress.city = "NewYork";
        //            billingAddress.country_code = "US";
        //            billingAddress.line1 = "23rd street kew gardens";
        //            billingAddress.postal_code = "43210";
        //            billingAddress.state = "NY";


        ////            Credit Card Number:
        ////            4225912489735107
        ////Credit Card Type:
        ////            VISA
        ////            Expiration Date:
        ////            04 / 2022

        //            //Now Create an object of credit card and add above details to it
        //            CreditCard crdtCard = new CreditCard();
        //            crdtCard.billing_address = billingAddress;
        //          //  crdtCard.cvv2 = "874";  //removed
        //            //crdtCard.expire_month = 1;
        //            crdtCard.expire_month = 4;
        //            crdtCard.expire_year = 2022;
        //           // crdtCard.first_name = "Aman";
        //           // crdtCard.last_name = "Thakur";
        //            //crdtCard.number = "1234567890123456";
        //            crdtCard.number = "4225912489735107";
        //           // crdtCard.type = "discover";
        //            crdtCard.type = "visa";



        //            // Specify details of your payment amount.
        //            Details details = new Details();
        //            details.shipping = "1";
        //            details.subtotal = "5";
        //            details.tax = "1";

        //            // Specify your total payment amount and assign the details object
        //            Amount amnt = new Amount();
        //            amnt.currency = "USD";
        //            // Total = shipping tax + subtotal.
        //            amnt.total = "7";
        //            amnt.details = details;

        //            // Now make a trasaction object and assign the Amount object
        //            Transaction tran = new Transaction();
        //            tran.amount = amnt;
        //            tran.description = "Description about the payment amount.";
        //            tran.item_list = itemList;
        //            tran.invoice_number = "your invoice number which you are generating";

        //            // Now, we have to make a list of trasaction and add the trasactions object
        //            // to this list. You can create one or more object as per your requirements

        //            List<Transaction> transactions = new List<Transaction>();
        //            transactions.Add(tran);

        //            // Now we need to specify the FundingInstrument of the Payer
        //            // for credit card payments, set the CreditCard which we made above

        //            FundingInstrument fundInstrument = new FundingInstrument();
        //            fundInstrument.credit_card = crdtCard;

        //            // The Payment creation API requires a list of FundingIntrument

        //            List<FundingInstrument> fundingInstrumentList = new List<FundingInstrument>();
        //            fundingInstrumentList.Add(fundInstrument);

        //            // Now create Payer object and assign the fundinginstrument list to the object
        //            Payer payr = new Payer();
        //            payr.funding_instruments = fundingInstrumentList;
        //            payr.payment_method = "credit_card";

        //            // finally create the payment object and assign the payer object & transaction list to it
        //            Payment pymnt = new Payment();
        //            pymnt.intent = "sale";
        //            pymnt.payer = payr;
        //            pymnt.transactions = transactions;

        //            try
        //            {
        //                //getting context from the paypal, basically we are sending the clientID and clientSecret key in this function 
        //                //to the get the context from the paypal API to make the payment for which we have created the object above.

        //                //Code for the configuration class is provided next

        //                // Basically, apiContext has a accesstoken which is sent by the paypal to authenticate the payment to facilitator account. An access token could be an alphanumeric string

        //                APIContext apiContext = Configuration.GetAPIContext();

        //                apiContext.ResetRequestId();
        //                // Create is a Payment class function which actually sends the payment details to the paypal API for the payment. The function is passed with the ApiContext which we received above.

        //                Payment createdPayment = pymnt.Create(apiContext);

        //                //if the createdPayment.State is "approved" it means the payment was successfull else not

        //                if (createdPayment.state.ToLower() != "approved")
        //                {
        //                    return View("FailureView");
        //                }
        //            }
        //            catch (PayPal.PayPalException ex)
        //            {
        //                Logger.Log("Error: " + ex.Message);
        //                return View("FailureView");
        //            }

        //            return View("SuccessView");
        //        }



        public ActionResult PaymentWithPaypal(int id)
        {
            //getting the apiContext as earlier
            APIContext apiContext = Configuration.GetAPIContext();
            //   int id = 218;
            string errorViewUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/Paypal/Error/";
            string successViewUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/Paypal/Success/";
            if (id == 0)
            {
                errorViewUrl += "Not_Found_Advertisement";
                return Redirect(errorViewUrl);
            }



            try
            {
                string payerId = Request.Params["PayerID"];

                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist

                    //it is returned by the create function call of the payment class

                    // Creating a payment

                    // baseURL is the url on which paypal sendsback the data.

                    // So we have provided URL of this controller only


                    //"+ id + "

                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Paypal/PaymentWithPayPal/" +
                                     id + "?";
                    //guid we are generating for storing the paymentID received in session

                    //after calling the create function and it is used in the payment execution

                    var guid = Convert.ToString((new Random()).Next(100000));

                    //CreatePayment function gives us the payment approval url

                    //on which payer is redirected for paypal acccount payment

                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid, id);

                    if (createdPayment == null)
                    {
                        errorViewUrl += "invalid_Payment_Creation";
                        return Redirect(errorViewUrl);
                    }
                   
                
                    //get links returned from paypal in response to Create function call

                    var links = createdPayment.links.GetEnumerator();

                    string paypalRedirectUrl = null;

                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment
                            paypalRedirectUrl = lnk.href;
                        }
                    }

                    // saving the paymentID in the key guid
                    Session.Add(guid, createdPayment.id);

                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This section is executed when we have received all the payments parameters

                    // from the previous call to the function Create

                    // Executing a payment

                    var guid = Request.Params["guid"];
                    string paymentId = Session[guid] as string;
                    var executedPayment = ExecutePayment(apiContext, payerId, paymentId);

                    if (executedPayment.state.ToLower() != "approved")
                    {
                        errorViewUrl += "Not_Approved_Payment";
                        return Redirect(errorViewUrl);
                    }
                    else if (executedPayment.state.ToLower() != "approved")
                    {
                        successViewUrl += paymentId;
                        return Redirect(successViewUrl);
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.Log("Error" + ex.Message);
                errorViewUrl += "Exeption_Payment";
                return Redirect(errorViewUrl);
                //return View("FailureView");
            }

            return View("SuccessView");
        }

        private Payment payment;

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
        }

        private Payment CreatePayment(APIContext apiContext, string redirectUrl,int id)
        {
            Tuple<decimal?, string, string, decimal> tuple = GetAdvertisementDetails(id);
            if (tuple == null)
                return null;

            string name = tuple.Item3;
            string price = Math.Round(tuple.Item4,2).ToString(CultureInfo.InvariantCulture);

            //similar to credit card create itemlist and add item objects to it
            var itemList = new ItemList() { items = new List<Item>() };

            itemList.items.Add(new Item()
            {
                name = name,
                currency = "USD",
                price = price,
                quantity = "1",
                sku = id.ToString()
            });

            var payer = new Payer() { payment_method = "paypal" };

            // Configure Redirect Urls here with RedirectUrls object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };

            // similar as we did for credit card, do here and create details object
            var details = new Details()
            {
                tax = "0",
                shipping = "0",
                subtotal = price
            };

            // similar as we did for credit card, do here and create amount object
            var amount = new Amount()
            {
                currency = "USD",
                total = price, // Total must be equal to sum of shipping, tax and subtotal.
                details = details
            };

            var transactionList = new List<Transaction>();

            transactionList.Add(new Transaction()
            {
                description = "Pay money for your advertisement.",
                invoice_number = new Random().Next(999999).ToString(),
                amount = amount,
                item_list = itemList
            });

            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            // Create a payment using a APIContext
            return this.payment.Create(apiContext);

        }

        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {

            //similar to credit card create itemlist and add item objects to it
            var itemList = new ItemList() { items = new List<Item>() };

            itemList.items.Add(new Item()
            {
                name = "Item Name",
                currency = "USD",
                price = "15",
                quantity = "5",
                sku = "sku"
            });

            var payer = new Payer() { payment_method = "paypal" };

            // Configure Redirect Urls here with RedirectUrls object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };

            // similar as we did for credit card, do here and create details object
            var details = new Details()
            {
                tax = "0",
                shipping = "0",
                subtotal = "75"
            };

            // similar as we did for credit card, do here and create amount object
            var amount = new Amount()
            {
                currency = "USD",
                total = "110.00", // Total must be equal to sum of shipping, tax and subtotal.
                details = details
            };

            var transactionList = new List<Transaction>();

            transactionList.Add(new Transaction()
            {
                description = "Transaction description.",
                invoice_number = new Random().Next(999999).ToString(),
                amount = amount,
                item_list = itemList
            });

            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            // Create a payment using a APIContext
            return this.payment.Create(apiContext);

        }

        public Tuple<decimal?, string, string,decimal> GetAdvertisementDetails(int id)
        {
            AdvertisementDto ads = _unitOfWork.Advertisements.GetSingleData(id);
            if (!ads.IsExpired)
                return null;
            string description = " قيمة اعلان مدفوع باسم";
            if (ads.IsPaided != null && ads.IsPaided.Value)
            {
                decimal? price = 0;
                string duration = ads.AdvertisementPeriod;

                price = ads.AdvertisementPrice;
                description += ads.Name;
                description += " لمدة ";
                description += duration;
                description += " يوم ";
                description += " بقيمة ";
                description += price;
                description += " ريال ";
                return new Tuple<decimal?, string, string,decimal>(price, duration, description, GetCurrencyValue(price.ToString()));

            }
            else
                return null;

        }

        
        public decimal GetCurrencyValue(string amount)
        {
            string fromCurrency = "SAR";
            string toCurrency = "USD";          

         WebClient web = new WebClient();
        string url = string.Format("https://www.google.com/finance/converter?a={2}&from={0}&to={1}", fromCurrency.ToUpper(),
            toCurrency.ToUpper(), amount);


        string response = web.DownloadString(url);

        var split = response.Split((new string[] { "<span class=bld>" }), StringSplitOptions.None);
        var value = split[1].Split(' ')[0];
        decimal rate = decimal.Parse(value, CultureInfo.InvariantCulture);

            return rate;
        }



        public ActionResult Error(string error)
        {
             return View("FailureView");

        }
        public ActionResult Success(string paymentId)
        {
            return View("SuccessView");

        }



        private static Currency GetCurrency(string value)
        {
            return new Currency() { value = value, currency = "USD" };
        }

        public static Plan CreatePlanObject(HttpContext httpContext)
        {

            
            // ### Create the Billing Plan
            // Both the trial and standard plans will use the same shipping
            // charge for this example, so for simplicity we'll create a
            // single object to use with both payment definitions.
            var shippingChargeModel = new ChargeModel()
            {
                type = "SHIPPING",
                amount = GetCurrency("9.99")
            };

            // Define the plan and attach the payment definitions and merchant preferences.
            // More Information: https://developer.paypal.com/webapps/developer/docs/api/#create-a-plan
            return new Plan
            {
                name = "T-Shirt of the Month Club Plan",
                description = "Monthly plan for getting the t-shirt of the month.",
                type = "fixed",
                // Define the merchant preferences.
                // More Information: https://developer.paypal.com/webapps/developer/docs/api/#merchantpreferences-object
                merchant_preferences = new MerchantPreferences()
                {
                    setup_fee = GetCurrency("1"),
                    return_url = httpContext.Request.Url.ToString(),
                    cancel_url = httpContext.Request.Url.ToString() + "?cancel",
                    auto_bill_amount = "YES",
                    initial_fail_amount_action = "CONTINUE",
                    max_fail_attempts = "0"
                },
                payment_definitions = new List<PaymentDefinition>
                {
                    // Define a trial plan that will only charge $9.99 for the first
                    // month. After that, the standard plan will take over for the
                    // remaining 11 months of the year.
                    new PaymentDefinition()
                    {
                        name = "Trial Plan",
                        type = "TRIAL",
                        frequency = "MONTH",
                        frequency_interval = "1",
                        amount = GetCurrency("9.99"),
                        cycles = "1",
                        charge_models = new List<ChargeModel>
                        {
                            new ChargeModel()
                            {
                                type = "TAX",
                                amount = GetCurrency("1.65")
                            },
                            shippingChargeModel
                        }
                    },
                    // Define the standard payment plan. It will represent a monthly
                    // plan for $19.99 USD that charges once month for 11 months.
                    new PaymentDefinition
                    {
                        name = "Standard Plan",
                        type = "REGULAR",
                        frequency = "MONTH",
                        frequency_interval = "1",
                        amount = GetCurrency("19.99"),
                        // > NOTE: For `IFNINITE` type plans, `cycles` should be 0 for a `REGULAR` `PaymentDefinition` object.
                        cycles = "11",
                        charge_models = new List<ChargeModel>
                        {
                            new ChargeModel
                            {
                                type = "TAX",
                                amount = GetCurrency("2.47")
                            },
                            shippingChargeModel
                        }
                    }
                }
            };
        }
        

        public ActionResult CreatePlan()
        {
          var apiContext = Configuration.GetAPIContext();
          var plan = CreatePlanObject(System.Web.HttpContext.Current);
            var createdPlan = plan.Create(apiContext);

            return View();

        }

        public ActionResult GetAllPlans()
        {
            var apiContext = Configuration.GetAPIContext();
            var planList = Plan.List(apiContext);
            string json = new JavaScriptSerializer().Serialize(planList);

            ViewBag.planList = Common.FormatJsonString(json);
            return View();
        }


        private Plan BillingPlanGet()
        {
            var planId = "P-9C6679336F8537830GQPZD6Y";
            var plan = Plan.Get(Configuration.GetAPIContext(), planId);
            return plan;

        }

        public ActionResult CreateBillingAgreement()
        {
            APIContext apiContext = Configuration.GetAPIContext();

            try
            {
                string id = Request.Params["guid"];

                if (string.IsNullOrEmpty(id))
                {

                    var createdPlan = BillingPlanGet();
                    var guid = Convert.ToString((new Random()).Next(100000));
                    createdPlan.merchant_preferences.return_url = Request.Url.ToString() + "?guid=" + guid;

                    // Activate the plan


                    //        var patchRequest = new PatchRequest()
                    //{
                    //    new Patch()
                    //    {
                    //        op = "replace",
                    //        path = "/",
                    //        value = new Plan() { state = "ACTIVE" }
                    //    }
                    //};



                    //        createdPlan.Update(apiContext, patchRequest);




                    string dt = DateTime.UtcNow.AddMinutes(1).ToString("yyyy-MM-ddTHH:mm:ssZ");
                    // With the plan created and activated, we can now create the billing agreement.
                    var payer = new Payer() { payment_method = "paypal" };
                    var shippingAddress = new ShippingAddress()
                    {
                        line1 = "111 First Street",
                        city = "Saratoga",
                        state = "CA",
                        postal_code = "95070",
                        country_code = "US"
                    };

                    var agreement = new Agreement()
                    {
                        name = "T-Shirt of the Month Club",
                        description = "Agreement for T-Shirt of the Month Club",
                        start_date = dt,
                        payer = payer,
                        plan = new Plan() { id = createdPlan.id },
                        shipping_address = shippingAddress
                        
                    };



                    // Create the billing agreement.
                    var createdAgreement = agreement.Create(apiContext);


                    // Get the redirect URL to allow the user to be redirected to PayPal to accept the agreement.
                    var links = createdAgreement.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        var link = links.Current;
                        if (link.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            paypalRedirectUrl = link.href;

                            //this.flow.RecordRedirectUrl("Redirect to PayPal to approve billing agreement...", link.href);
                        }
                    }
                    Session.Add(guid, createdAgreement.token);

                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This section is executed when we have received all the payments parameters

                    // from the previous call to the function Create

                    // Executing a payment

                    var guid = Request.Params["guid"];

                    var agreement = new Agreement() { token = Session[guid] as string };
                    var executedAgreement = agreement.Execute(apiContext);

                    if (executedAgreement.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                    string json = new JavaScriptSerializer().Serialize(executedAgreement);

                    ViewBag.Aggrement = Common.FormatJsonString(json);

                }
            }
            catch (Exception ex)
            {
                Logger.Log("Error" + ex.Message);
                return View("FailureView");
            }

            return View();









            // Before we can setup the billing agreement, we must first create a
            // billing plan that includes a redirect URL back to this test server.
            //var plan = CreatePlanObject(System.Web.HttpContext .Current);
            //var guid = Convert.ToString((new Random()).Next(100000));
            //plan.merchant_preferences.return_url = Request.Url.ToString() + "?guid=" + guid;



            

            //Session.Add("flow-" + guid, this.flow);
            

        }



    }
}
