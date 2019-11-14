using System;
using SAPI.Models;
using System.Collections.Generic;
using RestSharp;
using System.Management.Automation;

namespace SAPI
{
    public class Client
    {
        private RestClient RestClient;

        public bool IsAuthenticated { get; private set; }

        public TokenResponse Token { get; set; }
        // 65D38d873945s2T

        public Client(PSCredential credential)
        {
            RestClient = new RestClient("https://bokning.shoppisostersund.se/");
            var request = new RestRequest(Method.POST);
            request.Resource = "api/api/v1/token";
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(new TokenRequest
            {
                Email = credential.UserName,
                Password = credential.GetNetworkCredential().Password,
                GrantType = "password",
                Scope = "profile email"
            });

            var response = RestClient.Execute<TokenResponse>(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Token = response.Data;
                IsAuthenticated = true;
            }
            else
            {
                throw new InvalidCredentialsException();
            }
        }

        public List<ReservationResponse> Reservations(int skip = 0, int take = 25)
        {
            if (!IsAuthenticated)
                throw new NotAuthenticatedException();

            var request = new RestRequest(Method.GET);
            request.RequestFormat = DataFormat.Json;
            request.Resource = "api/api/v1/reservation?skip={skip}&sort=reservationNo&take={take}";
            request.AddHeader("Authorization", "Bearer " + Token.Token);
            request.AddUrlSegment("skip", skip);
            request.AddUrlSegment("take", take);

            IRestResponse<List<ReservationResponse>> response = RestClient.Execute<List<ReservationResponse>>(request);
            
            return response.Data;
        }

        public (List<ProductResponse>, int) Products(int skip = 0, int take = 25)
        {
            if (!IsAuthenticated)
                throw new NotAuthenticatedException();

            var request = new RestRequest(Method.GET);
            request.RequestFormat = DataFormat.Json;
            request.Resource = "api/api/v1/product?skip={skip}&sort=-productNo&take={take}";
            request.AddHeader("Authorization", "Bearer " + Token.Token);
            request.AddUrlSegment("skip", skip);
            request.AddUrlSegment("take", take);

            List<Product> pList = new List<Product>();
            IRestResponse<List<ProductResponse>> response = RestClient.Execute<List<ProductResponse>>(request);

            int TotalCount = 0;
            foreach (Parameter hdr in response.Headers)
            {
                if (hdr.Name.Equals("X-TotalCount"))
                    TotalCount = Int32.Parse(hdr.Value.ToString());
            }
            /*
            foreach (ReservationResponse rr in response.Data)
            {
                rList.Add(new Reservation
                {
                    Nr = rr.ReservationNo,
                    Cubicle = rr.reservableObject.Name,
                    Customer = rr.Customer.Name,
                    From = rr.ReservedFrom,
                    To = rr.ReservedTo,
                });
            }
            */

            return (response.Data, TotalCount);
        }

        public (List<ProductResponse>, int) AllProducts()
        {
            if (!IsAuthenticated)
                throw new NotAuthenticatedException();

            (List<ProductResponse> pList, int totalItems) = Products(0, 200);
            while (pList.Count - 1 < totalItems)
            {
                (List<ProductResponse> tPList, int tTotalItems) = Products(pList.Count - 1, 200);
                pList.AddRange(tPList);
            }
            return (pList, totalItems);
        }

        public (List<Product>, int) SearchProducts(string query, int skip, int take)
        {
            if (!IsAuthenticated)
                throw new NotAuthenticatedException();

            var request = new RestRequest(Method.GET);
            request.RequestFormat = DataFormat.Json;
            request.Resource = "api/api/v1/product?skip={skip}&sort=-productNo&take={take}&query={query}";
            request.AddHeader("Authorization", "Bearer " + Token.Token);
            request.AddUrlSegment("skip", skip);
            request.AddUrlSegment("take", take);
            request.AddUrlSegment("query", query);

            List<Product> pList = new List<Product>();
            IRestResponse<List<ProductResponse>> response = RestClient.Execute<List<ProductResponse>>(request);

            int TotalCount = 0;
            foreach (Parameter hdr in response.Headers)
            {
                if (hdr.Name.Equals("X-TotalCount"))
                    TotalCount = Int32.Parse(hdr.Value.ToString());
            }

            foreach (ProductResponse pr in response.Data)
            {
                if (pr.Price > 0)
                {
                    pList.Add(new Product
                    {
                        ProductNumber = pr.ProductNumber,
                        Name = pr.Name,
                        Price = pr.Price,
                        ProductCode = pr.ProductCode,
                        Status = StatusFromBool(pr.ReadOnly),
                        Cubicle = pr.ActiveReservableObjectName,
                        Total = SumFromSales(pr.Sales),
                    });
                }
            }
            return (pList, TotalCount);
        }

        private string StatusFromBool(bool value)
        {
            if (value)
                return "Sold";
            else
                return "New";
        }

        private float SumFromSales(List<Sale> sales)
        {
            float total = 0;
            foreach (Sale s in sales)
            {
                total += s.Amount;
            }
            return total;
        }
    }
}
