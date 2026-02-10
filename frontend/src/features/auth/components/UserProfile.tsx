import { LogoutConfirmDialog } from '@/components/LogoutConfirmDialog'
import { Button } from '@/components/ui/button'
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { useAuth } from '@/features/auth/hooks/useAuth'
import { useAuthStore } from '@/stores/auth-store'
import { zodResolver } from '@hookform/resolvers/zod'
import { Link, useNavigate } from '@tanstack/react-router'
import { ArrowLeft, Calendar, Edit2, LogOut, Mail, Save, User as UserIcon, X } from 'lucide-react'
import { useState } from 'react'
import { useForm } from 'react-hook-form'
import { toast } from 'sonner'
import { z } from 'zod'

const profileSchema = z.object({
    username: z
        .string()
        .min(3, 'Username must be at least 3 characters')
        .regex(/^[a-zA-Z0-9_]+$/, 'Username can only contain letters, numbers, and underscores'),
    email: z.string().email('Please enter a valid email address'),
})

type ProfileFormData = z.infer<typeof profileSchema>

export function UserProfile() {
    const { user, logout } = useAuth()
    const updateUser = useAuthStore((state) => state.updateUser)
    const navigate = useNavigate()
    const [isEditing, setIsEditing] = useState(false)
    const [showLogoutDialog, setShowLogoutDialog] = useState(false)

    const {
        register,
        handleSubmit,
        formState: { errors },
        reset,
    } = useForm<ProfileFormData>({
        resolver: zodResolver(profileSchema),
        defaultValues: {
            username: user?.username || '',
            email: user?.email || '',
        },
    })

    const handleLogout = () => {
        logout()
        toast.success('Logged out successfully', {
            description: 'You have been signed out of your account',
        })
        navigate({ to: '/login' })
    }

    const onSubmit = async (data: ProfileFormData) => {
        try {
            // TODO: Call API to update user profile
            // For now, just update local state
            updateUser(data)

            toast.success('Profile updated successfully', {
                description: 'Your profile information has been saved',
            })

            setIsEditing(false)
        } catch (error) {
            toast.error('Failed to update profile', {
                description: 'Please try again later',
            })
        }
    }

    const handleCancelEdit = () => {
        reset({
            username: user?.username || '',
            email: user?.email || '',
        })
        setIsEditing(false)
    }

    if (!user) {
        return (
            <div className="flex items-center justify-center min-h-screen">
                <Card className="w-full max-w-md">
                    <CardHeader>
                        <CardTitle>Not Authenticated</CardTitle>
                        <CardDescription>Please log in to view your profile</CardDescription>
                    </CardHeader>
                    <CardContent>
                        <Link to="/login">
                            <Button className="w-full">Go to Login</Button>
                        </Link>
                    </CardContent>
                </Card>
            </div>
        )
    }

    return (
        <div className="min-h-screen bg-gradient-to-br from-blue-50 via-indigo-50 to-purple-50 dark:from-gray-900 dark:via-blue-900/20 dark:to-purple-900/20 py-12 px-4">
            <div className="max-w-4xl mx-auto">
                {/* Back Button */}
                <Link to="/" className="inline-flex items-center gap-2 text-sm font-medium text-gray-600 hover:text-gray-900 dark:text-gray-400 dark:hover:text-gray-200 mb-6 transition-colors">
                    <ArrowLeft className="h-4 w-4" />
                    Back to Dashboard
                </Link>

                {/* Profile Header */}
                <Card className="mb-6 shadow-2xl border-0 bg-white/80 dark:bg-gray-900/80 backdrop-blur-xl overflow-hidden">
                    <div className="h-32 bg-gradient-to-r from-blue-600 via-violet-600 to-purple-600" />
                    <CardContent className="pt-0">
                        <div className="flex flex-col sm:flex-row items-center sm:items-end gap-6 -mt-16 pb-6">
                            {/* Avatar */}
                            <div className="relative">
                                <div className="w-32 h-32 rounded-full bg-gradient-to-br from-blue-500 to-purple-600 p-1 shadow-xl">
                                    <div className="w-full h-full rounded-full bg-white dark:bg-gray-900 flex items-center justify-center">
                                        <UserIcon className="h-16 w-16 text-blue-600 dark:text-blue-400" />
                                    </div>
                                </div>
                            </div>

                            {/* User Info */}
                            <div className="flex-1 text-center sm:text-left">
                                <h1 className="text-3xl font-bold bg-gradient-to-r from-blue-600 to-purple-600 bg-clip-text text-transparent mb-2">
                                    {user.username}
                                </h1>
                                <p className="text-gray-600 dark:text-gray-400 flex items-center gap-2 justify-center sm:justify-start">
                                    <Mail className="h-4 w-4" />
                                    {user.email}
                                </p>
                            </div>

                            {/* Edit Button */}
                            <Button
                                onClick={() => setIsEditing(!isEditing)}
                                variant={isEditing ? "outline" : "default"}
                                className={isEditing ? "" : "bg-gradient-to-r from-blue-600 to-purple-600 hover:from-blue-700 hover:to-purple-700"}
                            >
                                {isEditing ? (
                                    <>
                                        <X className="h-4 w-4 mr-2" />
                                        Cancel
                                    </>
                                ) : (
                                    <>
                                        <Edit2 className="h-4 w-4 mr-2" />
                                        Edit Profile
                                    </>
                                )}
                            </Button>
                        </div>
                    </CardContent>
                </Card>

                {/* Account Details */}
                <div className="grid grid-cols-1 md:grid-cols-2 gap-6 mb-6">
                    <Card className="shadow-xl border-0 bg-white/80 dark:bg-gray-900/80 backdrop-blur-xl">
                        <CardHeader>
                            <CardTitle className="text-xl">
                                {isEditing ? 'Edit Information' : 'Account Information'}
                            </CardTitle>
                            <CardDescription>
                                {isEditing ? 'Update your account details' : 'Your account details'}
                            </CardDescription>
                        </CardHeader>
                        <CardContent className="space-y-4">
                            {isEditing ? (
                                <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
                                    <div className="space-y-2">
                                        <Label htmlFor="username">Username</Label>
                                        <Input
                                            id="username"
                                            {...register('username')}
                                            className="h-11"
                                        />
                                        {errors.username && (
                                            <p className="text-sm text-red-600 dark:text-red-400">
                                                {errors.username.message}
                                            </p>
                                        )}
                                    </div>
                                    <div className="space-y-2">
                                        <Label htmlFor="email">Email</Label>
                                        <Input
                                            id="email"
                                            type="email"
                                            {...register('email')}
                                            className="h-11"
                                        />
                                        {errors.email && (
                                            <p className="text-sm text-red-600 dark:text-red-400">
                                                {errors.email.message}
                                            </p>
                                        )}
                                    </div>
                                    <div className="flex gap-3 pt-2">
                                        <Button
                                            type="submit"
                                            className="flex-1 bg-gradient-to-r from-blue-600 to-purple-600 hover:from-blue-700 hover:to-purple-700"
                                        >
                                            <Save className="h-4 w-4 mr-2" />
                                            Save Changes
                                        </Button>
                                        <Button
                                            type="button"
                                            variant="outline"
                                            onClick={handleCancelEdit}
                                        >
                                            Cancel
                                        </Button>
                                    </div>
                                </form>
                            ) : (
                                <>
                                    <div className="flex items-center gap-3 p-3 rounded-lg bg-gray-50 dark:bg-gray-800/50">
                                        <div className="p-2 rounded-lg bg-blue-100 dark:bg-blue-900/30">
                                            <UserIcon className="h-5 w-5 text-blue-600 dark:text-blue-400" />
                                        </div>
                                        <div>
                                            <p className="text-sm text-gray-500 dark:text-gray-400">Username</p>
                                            <p className="font-semibold">{user.username}</p>
                                        </div>
                                    </div>
                                    <div className="flex items-center gap-3 p-3 rounded-lg bg-gray-50 dark:bg-gray-800/50">
                                        <div className="p-2 rounded-lg bg-purple-100 dark:bg-purple-900/30">
                                            <Mail className="h-5 w-5 text-purple-600 dark:text-purple-400" />
                                        </div>
                                        <div>
                                            <p className="text-sm text-gray-500 dark:text-gray-400">Email</p>
                                            <p className="font-semibold">{user.email}</p>
                                        </div>
                                    </div>
                                    <div className="flex items-center gap-3 p-3 rounded-lg bg-gray-50 dark:bg-gray-800/50">
                                        <div className="p-2 rounded-lg bg-indigo-100 dark:bg-indigo-900/30">
                                            <Calendar className="h-5 w-5 text-indigo-600 dark:text-indigo-400" />
                                        </div>
                                        <div>
                                            <p className="text-sm text-gray-500 dark:text-gray-400">User ID</p>
                                            <p className="font-semibold font-mono text-sm">{user.id}</p>
                                        </div>
                                    </div>
                                </>
                            )}
                        </CardContent>
                    </Card>

                    <Card className="shadow-xl border-0 bg-white/80 dark:bg-gray-900/80 backdrop-blur-xl">
                        <CardHeader>
                            <CardTitle className="text-xl">Quick Actions</CardTitle>
                            <CardDescription>Manage your account</CardDescription>
                        </CardHeader>
                        <CardContent className="space-y-3">
                            <Button
                                variant="outline"
                                className="w-full justify-start h-auto py-3 hover:bg-red-50 dark:hover:bg-red-900/20 text-red-600 dark:text-red-400 hover:text-red-700 dark:hover:text-red-300 border-red-200 dark:border-red-900/50"
                                onClick={() => setShowLogoutDialog(true)}
                            >
                                <LogOut className="h-5 w-5 mr-3" />
                                <div className="text-left">
                                    <p className="font-semibold">Sign Out</p>
                                    <p className="text-xs opacity-70">Log out of your account</p>
                                </div>
                            </Button>
                        </CardContent>
                    </Card>
                </div>

                {/* Stats Card - Placeholder for future enhancement */}
                <Card className="shadow-xl border-0 bg-white/80 dark:bg-gray-900/80 backdrop-blur-xl">
                    <CardHeader>
                        <CardTitle className="text-xl">Account Statistics</CardTitle>
                        <CardDescription>Your expense tracking overview</CardDescription>
                    </CardHeader>
                    <CardContent>
                        <div className="grid grid-cols-3 gap-4">
                            <div className="text-center p-4 rounded-lg bg-gradient-to-br from-blue-50 to-indigo-50 dark:from-blue-900/20 dark:to-indigo-900/20">
                                <p className="text-3xl font-bold bg-gradient-to-r from-blue-600 to-indigo-600 bg-clip-text text-transparent">0</p>
                                <p className="text-sm text-gray-600 dark:text-gray-400 mt-1">Expenses</p>
                            </div>
                            <div className="text-center p-4 rounded-lg bg-gradient-to-br from-purple-50 to-pink-50 dark:from-purple-900/20 dark:to-pink-900/20">
                                <p className="text-3xl font-bold bg-gradient-to-r from-purple-600 to-pink-600 bg-clip-text text-transparent">0</p>
                                <p className="text-sm text-gray-600 dark:text-gray-400 mt-1">Budgets</p>
                            </div>
                            <div className="text-center p-4 rounded-lg bg-gradient-to-br from-green-50 to-emerald-50 dark:from-green-900/20 dark:to-emerald-900/20">
                                <p className="text-3xl font-bold bg-gradient-to-r from-green-600 to-emerald-600 bg-clip-text text-transparent">0</p>
                                <p className="text-sm text-gray-600 dark:text-gray-400 mt-1">Goals</p>
                            </div>
                        </div>
                    </CardContent>
                </Card>
            </div>

            {/* Logout Confirmation Dialog */}
            <LogoutConfirmDialog
                open={showLogoutDialog}
                onOpenChange={setShowLogoutDialog}
                onConfirm={handleLogout}
            />
        </div>
    )
}
