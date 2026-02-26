import { ArrowUpRight, Github, Linkedin, Mail, Twitter, Wallet } from 'lucide-react';

const footerLinks = {
    Product: [
        { name: 'Features', href: '#features' },
        { name: 'Pricing', href: '#pricing' },
        { name: 'Integrations', href: '#' },
        { name: 'Changelog', href: '#' },
        { name: 'Documentation', href: '#' },
    ],
    Company: [
        { name: 'About Us', href: '#' },
        { name: 'Blog', href: '#' },
        { name: 'Careers', href: '#', badge: 'Hiring' },
        { name: 'Press Kit', href: '#' },
        { name: 'Contact', href: '#' },
    ],
    Resources: [
        { name: 'Help Center', href: '#' },
        { name: 'Community', href: '#' },
        { name: 'Templates', href: '#' },
        { name: 'Guides', href: '#' },
        { name: 'API Reference', href: '#' },
    ],
    Legal: [
        { name: 'Privacy Policy', href: '#' },
        { name: 'Terms of Service', href: '#' },
        { name: 'Security', href: '#' },
        { name: 'Cookie Policy', href: '#' },
    ],
}

const socialLinks = [
    { icon: Twitter, href: '#', label: 'Twitter' },
    { icon: Github, href: '#', label: 'GitHub' },
    { icon: Linkedin, href: '#', label: 'LinkedIn' },
]

export function Footer() {
    return (
        <footer className="relative border-t border-gray-200/80 dark:border-gray-800/80 overflow-hidden">
            {/* Background */}
            <div className="absolute inset-0 -z-10 bg-gradient-to-b from-gray-50/50 via-gray-100/30 to-gray-50/50 dark:from-gray-950/50 dark:via-gray-900/30 dark:to-gray-950/50" />
            <div className="absolute bottom-0 left-1/4 w-96 h-96 bg-blue-500/5 rounded-full blur-3xl" />
            <div className="absolute bottom-0 right-1/4 w-96 h-96 bg-violet-500/5 rounded-full blur-3xl" />

            <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                {/* Main Footer */}
                <div className="py-16 lg:py-20 grid grid-cols-2 md:grid-cols-6 gap-10 lg:gap-12">
                    {/* Brand Column */}
                    <div className="col-span-2">
                        {/* Logo */}
                        <div className="flex items-center gap-3 mb-6">
                            <div className="relative">
                                <div className="absolute inset-0 bg-gradient-to-r from-blue-500 to-violet-500 rounded-xl blur-md opacity-40" />
                                <div className="relative bg-gradient-to-br from-blue-600 via-violet-600 to-fuchsia-600 p-2.5 rounded-xl shadow-lg">
                                    <Wallet className="h-5 w-5 text-white" />
                                </div>
                            </div>
                            <span className="text-xl font-black text-gray-900 dark:text-white">SpendWise</span>
                        </div>

                        <p className="text-sm text-gray-600 dark:text-gray-400 leading-relaxed max-w-xs mb-6">
                            The smart way to track expenses, manage budgets, and achieve your financial goals. Join 50,000+ users today.
                        </p>

                        {/* Newsletter */}
                        <div className="mb-6">
                            <p className="text-sm font-semibold text-gray-900 dark:text-white mb-3">Stay updated</p>
                            <div className="flex gap-2">
                                <div className="relative flex-1">
                                    <Mail className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-gray-400" />
                                    <input
                                        type="email"
                                        placeholder="Enter your email"
                                        className="w-full pl-10 pr-4 py-2.5 text-sm rounded-xl bg-white dark:bg-gray-800 border border-gray-200 dark:border-gray-700 focus:border-blue-500 dark:focus:border-blue-500 focus:ring-2 focus:ring-blue-500/20 transition-all outline-none"
                                    />
                                </div>
                                <button className="px-4 py-2.5 bg-gradient-to-r from-blue-600 to-violet-600 hover:from-blue-700 hover:to-violet-700 text-white text-sm font-semibold rounded-xl transition-all duration-300 hover:shadow-lg hover:shadow-blue-500/25">
                                    Subscribe
                                </button>
                            </div>
                        </div>

                        {/* Social Links */}
                        <div className="flex gap-2">
                            {socialLinks.map((social) => (
                                <SocialLink key={social.label} icon={<social.icon className="h-4 w-4" />} href={social.href} label={social.label} />
                            ))}
                        </div>
                    </div>

                    {/* Link Columns */}
                    {Object.entries(footerLinks).map(([category, links]) => (
                        <div key={category}>
                            <h4 className="font-bold text-gray-900 dark:text-white text-sm mb-5">{category}</h4>
                            <ul className="space-y-3">
                                {links.map((link) => (
                                    <li key={link.name}>
                                        <a
                                            href={link.href}
                                            className="group inline-flex items-center gap-2 text-sm text-gray-600 dark:text-gray-400 hover:text-gray-900 dark:hover:text-white transition-colors"
                                        >
                                            {link.name}
                                            {'badge' in link && link.badge && (
                                                <span className="px-2 py-0.5 text-[10px] font-bold bg-gradient-to-r from-blue-600 to-violet-600 text-white rounded-full">
                                                    {link.badge}
                                                </span>
                                            )}
                                            <ArrowUpRight className="h-3 w-3 opacity-0 -translate-x-1 group-hover:opacity-50 group-hover:translate-x-0 transition-all" />
                                        </a>
                                    </li>
                                ))}
                            </ul>
                        </div>
                    ))}
                </div>

                {/* Bottom Bar */}
                <div className="py-6 border-t border-gray-200/80 dark:border-gray-800/80 flex flex-col sm:flex-row items-center justify-between gap-4">
                    <p className="text-sm text-gray-500 dark:text-gray-500">
                        © {new Date().getFullYear()} SpendWise. All rights reserved.
                    </p>
                    <div className="flex items-center gap-6">
                        <a href="#" className="text-sm text-gray-500 hover:text-gray-700 dark:hover:text-gray-300 transition-colors">Privacy</a>
                        <a href="#" className="text-sm text-gray-500 hover:text-gray-700 dark:hover:text-gray-300 transition-colors">Terms</a>
                        <a href="#" className="text-sm text-gray-500 hover:text-gray-700 dark:hover:text-gray-300 transition-colors">Cookies</a>
                        <div className="flex items-center gap-1.5 text-sm text-gray-500">
                            <span className="w-2 h-2 rounded-full bg-emerald-500 animate-pulse" />
                            All systems operational
                        </div>
                    </div>
                </div>
            </div>
        </footer>
    )
}

function SocialLink({ icon, href, label }: { icon: React.ReactNode; href: string; label: string }) {
    return (
        <a
            href={href}
            aria-label={label}
            className="w-10 h-10 rounded-xl bg-gray-100 dark:bg-gray-800 border border-gray-200/50 dark:border-gray-700/50 flex items-center justify-center text-gray-600 dark:text-gray-400 hover:bg-gradient-to-br hover:from-blue-600 hover:to-violet-600 hover:text-white hover:border-transparent hover:shadow-lg hover:shadow-blue-500/25 transition-all duration-300"
        >
            {icon}
        </a>
    )
}
