using System;
using SAPI.Models;
using System.Collections.Generic;
using RestSharp;
using SAPI.Errors;

namespace SAPI
{
    public class Client
    {
        private RestClient RestClient;

        public bool IsAuthenticated { get; private set; }

        public TokenResponse Token { get; set; }

        public Client(string email, string password)
        {
            RestClient = new RestClient("https://bokning.shoppisostersund.se/");
            var request = new RestRequest(Method.POST);
            request.Resource = "api/api/v1/token";
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(new TokenRequest
            {
                Email = email,
                Password = password,
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

        public (List<T>, int) GetObjects<T>(int skip, int take) where T : Base, new()
        {
            if (!IsAuthenticated)
                throw new NotAuthenticatedException();

            var request = new RestRequest(Method.GET);
            request.RequestFormat = DataFormat.Json;
            switch (typeof(T).Name) {
                case "Reservation":
                    request.Resource = "api/api/v1/reservation?skip={skip}&sort=reservationNo&take={take}";
                    break;
                case "Product":
                    request.Resource = "api/api/v1/product?skip={skip}&sort=-productNo&take={take}";
                    break;
                case "Transaction":
                    request.Resource = "api/api/v1/customer/transactions?skip={skip}&sort=-time&query=&take={take}";
                    break;
                default:
                    throw new InvalidObjectTypeException();
            }
            
            request.AddHeader("Authorization", "Bearer " + Token.Token);
            request.AddUrlSegment("skip", skip);
            request.AddUrlSegment("take", take);

            IRestResponse<List<T>> response = RestClient.Execute<List<T>>(request);

            int TotalCount = 0;
            foreach (Parameter hdr in response.Headers)
            {
                if (hdr.Name.Equals("X-TotalCount"))
                    TotalCount = Int32.Parse(hdr.Value.ToString());
            }

            return (response.Data, TotalCount);
        }

        public (List<T>, int) GetObjects<T>(int skip, int take, string query) where T : Base, new()
        {
            if (!IsAuthenticated)
                throw new NotAuthenticatedException();

            var request = new RestRequest(Method.GET);
            request.RequestFormat = DataFormat.Json;
            switch (typeof(T).Name)
            {
                case "Reservation":
                    request.Resource = "api/api/v1/reservation?skip={skip}&sort=reservationNo&query={query}&take={take}";
                    break;
                case "Product":
                    request.Resource = "api/api/v1/product?skip={skip}&sort=-productNo&query={query}&take={take}";
                    break;
                case "Transaction":
                    request.Resource = "api/api/v1/customer/transactions?skip={skip}&sort=-time&query={query}&take={take}";
                    break;
                default:
                    throw new InvalidObjectTypeException();
            }

            request.AddHeader("Authorization", "Bearer " + Token.Token);
            request.AddUrlSegment("skip", skip);
            request.AddUrlSegment("take", take);

            IRestResponse<List<T>> response = RestClient.Execute<List<T>>(request);

            int TotalCount = 0;
            foreach (Parameter hdr in response.Headers)
            {
                if (hdr.Name.Equals("X-TotalCount"))
                    TotalCount = Int32.Parse(hdr.Value.ToString());
            }

            return (response.Data, TotalCount);
        }

        public (List<T>, int) GetAllObjects<T>() where T : Base, new()
        {
            if (!IsAuthenticated)
                throw new NotAuthenticatedException();

            (List<T> pList, int totalItems) = GetObjects<T>(0, 200);
            while (pList.Count - 1 < totalItems)
            {
                (List<T> tPList, int tTotalItems) = GetObjects<T>(pList.Count - 1, 200);
                pList.AddRange(tPList);
            }
            return (pList, totalItems);
        }
    }
}
