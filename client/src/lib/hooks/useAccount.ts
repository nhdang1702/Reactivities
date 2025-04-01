import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query"
import { LoginSchema } from "../schemas/loginSchema"
import agent from "../api/agent"
import { useLocation, useNavigate } from "react-router"
import { RegisterSchema } from "../schemas/registerSchema"
import { toast } from "react-toastify"

export const useAccount = () => {
    const navigate = useNavigate();
    const location = useLocation();
    const queryClient = useQueryClient();
    const loginUser = useMutation({
        mutationFn: async (creds: LoginSchema) => {
            await agent.post('/login?useCookies=true', creds);
        },
        onSuccess: async () => {
            await queryClient.invalidateQueries({
                queryKey: ['user']
            })
        }
    });
    const logoutUser = useMutation({
        mutationFn: async () => {
            await agent.post('/account/logout')
        },
        onSuccess: () => {
            queryClient.removeQueries({
                queryKey: ['user']
            })
            queryClient.removeQueries({
                queryKey: ['activities']
            })
            navigate('/')
        }
    });

    const registerUser = useMutation({
        mutationFn: async (creds: RegisterSchema) => {
            await agent.post('/account/register', creds)
        },
        onSuccess: () => {
            toast.success("Register successful - Now you can login");
            navigate('/login');
        }
    })

    const {data: currentUser, isLoading: loadingUserInfo} = useQuery({
        queryKey: ['user'],
        queryFn: async () => {
            const response = await agent.get<User>('/account/user-info');
            return response.data;
        },
        enabled: !queryClient.getQueryData(['user']) && location.pathname !== '/login'
                    && location.pathname !== '/register'
        
    })

    return {
        loginUser, currentUser, logoutUser, loadingUserInfo, registerUser
    }
}