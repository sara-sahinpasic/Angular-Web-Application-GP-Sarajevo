namespace Application.Localization;

public sealed class Messages
{
    public Dictionary<string, string> Bs { get; } = new() {
        // auth controller
        { "auth_controller_register_action_user_registered_error", "Korisnik je već registrovan." },
        { "auth_controller_register_action_success", "Uspješna registrovan korisnik." },

        { "auth_controller_activate_action_invalid_token", "Aktivacijski token nije validan." },
        { "auth_controller_activate_action_expired_token_error", "Token je istekao. Novi token Vam je poslan na e-mail." },

        { "auth_controller_login_action_user_not_found", "Korisnik nije pronađen." },
        { "auth_controller_login_action_account_not_active", "Račun nije aktivan. Provjerite e-mail za aktivacijski kod." },
        { "auth_controller_login_action_two_way_auth", "Verifikacijski kod je poslan na e-mail." },
        { "auth_controller_login_action_email_cannot_be_empty", "E-mail polje ne smije biti prazno." },
        { "auth_controller_login_action_password_cannot_be_empty", "Lozinka ne smije biti prazna." },
        { "auth_controller_login_action_login_success", "Uspješna prijava." },

        { "auth_controller_authenticate_login_incorrect_login_code", "Verifikacijski kod neispravan. Pokušajte ponovo." },
        { "auth_controller_authenticate_login_code_expired", "Verifikacijski kod istekao." },

        { "auth_controller_reset_password_error_empty_email_field", "E-mail polje ne smije biti prazno." },
        { "auth_controller_reset_password_success", "Nova lozinka Vam je poslana na e-mail." },

        // profile controller
        { "profile_controller_update_profile_no_user_found", "Korisnik nije pronađen." },
        { "profile_controller_update_profile_file_extension_error", "Nije validna ekstenzija fajla." },
        { "profile_controller_update_profile_update_success", "Korisnik uspješno ažuriran." },

        { "profile_controller_delete_profile_no_user_found", "Korisnik nije pronađen." },
        { "profile_controller_delete_profile_success", "Korisnik uspješno obrisan." },
        
        // request controller
        { "request_controller_send_request_action_multiple_requests_error", "Jedan zahtjev je već u obradi." },
        { "request_controller_send_request_action_file_extension_error", "Nije validna ekstenzija fajla." },
        { "request_controller_send_request_action_success", "Uspješno poslan zahtjev." },

        // checkout controller
        { "checkout_controller_checkout_no_user_found_error", "Nije bilo moguće okončati kupovinu. Pokušajte se ulogovati prvo." },
        { "checkout_controller_checkout_faulty_data", "Pogrešni podaci. Molimo, pokušajte ponovo." },
        { "checkout_controller_checkout_success", "Uspjeh! Račun i karte su Vam poslani na e-mail. Provjerite i svoj Spam folder." },
        { "checkout_controller_checkout_faulty_route", "Unesena je nepostojeća ruta." },

        // news controller
        { "news_controller_get_news_by_id_no_news_found_error", "Obavjesti nisu pronađene." },

        // review controller
        { "review_controller_add_review_no_purchased_tickets_error", "Niste kupili niti jednu kartu." },
        { "review_controller_add_review_success", "Uspješno ste ostavili utisak." },
        { "review_controller_add_review_invalid_input", "Unos nije validan. Molimo Vas, provjerite Vaš unos." },

        // route controller
        { "route_controller_no_routes_found_message", "Nije pronađena niti jedna aktivna ruta za zadani period." },

        // error controller
        { "error_controller_general_error", "Došlo je do greške." }
    };

    public Dictionary<string, string> En { get; } = new() {
        // auth controller
        { "auth_controller_register_action_user_registered_error", "User is already registered." },
        { "auth_controller_register_action_success", "User successfully created." },

        { "auth_controller_activate_action_invalid_token", "Activation token not valid." },
        { "auth_controller_activate_action_expired_token_error", "Token has expired. A new one has been sent." },

        { "auth_controller_login_action_user_not_found", "User with those credentials not found." },
        { "auth_controller_login_action_account_not_active", "Account not activated. Check your email for the activation code." },
        { "auth_controller_login_action_email_cannot_be_empty", "The e-mail field cannot be empty." },
        { "auth_controller_login_action_password_cannot_be_empty", "The password field cannot be empty." },
        { "auth_controller_login_action_two_way_auth", "Verification code sent to email." },
        { "auth_controller_login_action_login_success", "Successfully logged in." },

        { "auth_controller_authenticate_login_incorrect_login_code", "Login code not correct. Try again." },
        { "auth_controller_authenticate_login_code_expired", "Login code expired." },

        { "auth_controller_reset_password_error_empty_email_field", "The email field cannot be empty." },
        { "auth_controller_reset_password_success", "You can check your email now for a new password." },

        // profile controller
        { "profile_controller_update_profile_no_user_found", "No user found." },
        { "profile_controller_update_profile_file_extension_error", "File extension not valid." },
        { "profile_controller_update_profile_update_success", "User successfully updated!" },

        { "profile_controller_delete_profile_no_user_found", "No user found." },
        { "profile_controller_delete_profile_success", "User successfully deleted." },
        
        // request controller
        { "request_controller_send_request_action_multiple_requests_error", "One request has been sent already. Please, wait until it's processed." },
        { "request_controller_send_request_action_file_extension_error", "File extension not valid." },
        { "request_controller_send_request_action_success", "Sent successfully." },

        // checkout controller
        { "checkout_controller_checkout_no_user_found_error", "Cannot complete purchase as the user was not found. Try to login first." },
        { "checkout_controller_checkout_faulty_data", "Faulty data. Please, try again." },
        { "checkout_controller_checkout_success", "Success! Your tickets and invoice will arrive in your email inbox shortly. Check your spam folder if you cannot find it." },
        { "checkout_controller_checkout_faulty_route", "Unknown route has been provided." },

        // news controller
        { "news_controller_get_news_by_id_no_news_found_error", "No news was found." },

        // review controller
        { "review_controller_add_review_no_purchased_tickets_error", "You haven't purchased any Tickets." },
        { "review_controller_add_review_success", "You have successfully left a review." },
        { "review_controller_add_review_invalid_input", "Invalid input. Please, check your data." },
        
        // route controller
        { "route_controller_no_routes_found_message", "No active routes were found for the selected time period." },
        
        // error controller
        { "error_controller_general_error", "An error occurred." }
    };
}
